using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SalaryManagement.Utility;
using System.Text;

namespace SalaryManagement
{
    public partial class SalaryUC : UserControl
    {
        public SalaryUC()
        {
            InitializeComponent();
        }

        private void btSearch_Click(object sender, EventArgs e)
        {

        }

        private void btUpload_Click(object sender, EventArgs e)
        {
            this.lblMsg.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(tbYM.Text))
            {
                this.lblMsg.Text = "月份不能为空。";
                return;
            }

            DateTime dtYM = new DateTime();
            if (!DateTime.TryParseExact(tbYM.Text,
                           "yyyyMM",
                           System.Globalization.CultureInfo.InvariantCulture,
                           System.Globalization.DateTimeStyles.None,
                           out dtYM))
            {
                this.lblMsg.Text = "月份输入格式不正确。";
                return;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "xls文件 (*.xls)|*.xls|xlsx文件 (*.xlsx)|*.xlsx|所有文件 (*.*)|*.*"; // file types, that will be allowed to upload
            dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                doUpload(dialog.FileName, dtYM.ToString("yyyyMM"));
            }
        }

        private void doUpload(string fileName, string sYM)
        {
            using (FileStream stream = File.Open(fileName, FileMode.Open))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rows = sheet.GetRowEnumerator();
                rows.MoveNext();
                HSSFRow row = (HSSFRow)rows.Current;

                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnectionString);

                try
                {
                    OleDbCommand dbCommand = new OleDbCommand();
                    dbCommand.Connection = conn;
                    dbCommand.CommandText = "select count(1) from Salary where YM = '" + sYM + "'";
                    conn.Open();

                    if ((int)dbCommand.ExecuteScalar() > 0)
                    {
                        DialogResult dlgRes = MessageBox.Show(
                                  sYM + "员工工资数据已经存在，点击确认替换存在的数据？",
                                  "替换确认",
                                  MessageBoxButtons.YesNo,
                                  MessageBoxIcon.Question);
                        if (dlgRes == DialogResult.Yes)
                        {
                            dbCommand.CommandText = "delete from Salary where YM = '" + sYM + "'";
                            dbCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            return;
                        }
                    }

                    #region 新增列
                    SalaryManagementDataSetTableAdapters.SalarySettingTableAdapter ssAdapter = new SalaryManagementDataSetTableAdapters.SalarySettingTableAdapter();
                    SalaryManagementDataSet.SalarySettingDataTable ssDataTable = new SalaryManagementDataSet.SalarySettingDataTable();
                    ssAdapter.Fill(ssDataTable);
                    DataRowCollection ssRowCollection = ssDataTable.Rows;
                    int? maxSeq = ssAdapter.SelectMaxSeqBy类型("应发");

                    OleDbDataAdapter dbAdapter = new OleDbDataAdapter("select * from Salary", conn);
                    DataSet dataSet = new DataSet();
                    dbAdapter.FillSchema(dataSet, SchemaType.Mapped);
                    DataColumnCollection columnCollection = dataSet.Tables[0].Columns;

                    int columnIndex = 7;
                    bool hasError = false;
                    string headType = "应发";
                    StringBuilder insertField = new StringBuilder("insert into Salary (YM, StaffId");
                    IList<int> effColumnList = new List<int>();
                    IList<string> addHeadNameList = new List<string>();
                    IList<string> addHeadFieldList = new List<string>();
                    while (ImportHelper.GetCellStringValue(row.GetCell(columnIndex)) != null)
                    {
                        string headName = ImportHelper.GetCellStringValue(row.GetCell(columnIndex));
                        //if (headList.Contains(headName))
                        //{
                        //    this.lblMsg.Text = "第" + ImportHelper.GetExcelColumnName(columnIndex) + "列项目" + headName + "重复。";
                        //    hasError = true;
                        //    break;
                        //}
                        //else
                        //{
                        columnIndex++;
                        if (headName == "应发")
                        {
                            headType = "扣发";
                            maxSeq = ssAdapter.SelectMaxSeqBy类型("扣发");
                            continue;
                        }
                        else if (headName == "扣发")
                        {
                            continue;
                        }
                        else if (headName == "实发工资")
                        {
                            continue;
                        }

                        effColumnList.Add(columnIndex - 1);
                        //}

                        bool hasHead = false;
                        string headFieldName = null;
                        foreach (DataRow ssRow in ssRowCollection)
                        {
                            if ((string)ssRow[1] == headName || addHeadNameList.Contains(headName))
                            {
                                hasHead = true;
                                headFieldName = (string)ssRow[3];
                                break;
                            }
                        }
                        if (!hasHead)
                        {
                            if (!maxSeq.HasValue)
                            {
                                maxSeq = 0;
                            }
                            maxSeq++;
                            headFieldName = (headType + maxSeq);
                            addHeadNameList.Add(headName);
                            ssAdapter.Insert(headName, headType, headFieldName, maxSeq);
                        }

                        if (!columnCollection.Contains(headFieldName) && !addHeadFieldList.Contains(headFieldName))
                        {
                            addHeadFieldList.Add(headFieldName);
                            dbCommand.CommandText = "alter table Salary add column " + headFieldName + " decimal(18,8)";
                            dbCommand.ExecuteNonQuery();
                        }

                        insertField.Append(", " + headFieldName);
                    }
                    insertField.Append(")");
                    #endregion

                    #region
                    if (!hasError)
                    {
                        SalaryManagementDataSetTableAdapters.StaffTableAdapter stAdapter = new SalaryManagementDataSetTableAdapters.StaffTableAdapter();
                        int rowIndex = 1;
                        StringBuilder insertValue = null;
                        while (rows.MoveNext())
                        {
                            if (hasError)
                            {
                                break;
                            }

                            rowIndex++;
                            row = (HSSFRow)rows.Current;
                            if (!ImportHelper.CheckValidDataRow(row, 1, columnIndex))
                            {
                                break;//边界
                            }

                            #region 读取数据
                            string nameCell = ImportHelper.GetCellStringValue(row.GetCell(2));
                            int? pId = stAdapter.SelectIdBy姓名(nameCell);
                            if (!pId.HasValue)
                            {
                                this.lblMsg.Text = "第" + rowIndex + "行员工姓名不存在。";
                                hasError = true;
                                break;
                            }
                            insertValue = new StringBuilder(" values ('" + sYM + "', " + pId); ;
                            foreach (int effColumnIndex in effColumnList)
                            {
                                string cell = ImportHelper.GetCellStringValue(row.GetCell(effColumnIndex));
                                decimal dCell = 0;
                                if (!string.IsNullOrWhiteSpace(cell))
                                {
                                    if (!decimal.TryParse(cell, out dCell))
                                    {
                                        this.lblMsg.Text = "第" + rowIndex + "行第" + ImportHelper.GetExcelColumnName(effColumnIndex) + "列不是有效的数字类型。";
                                        hasError = true;
                                        break;
                                    }
                                }

                                insertValue.Append(", " + dCell.ToString());
                            }
                            insertValue.Append(")");
                            dbCommand.CommandText = insertField.ToString() + insertValue.ToString();
                            dbCommand.ExecuteNonQuery();
                            #endregion
                        }

                        this.lblMsg.Text = sYM + "员工工资数据导入成功。";

                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    this.lblMsg.Text = ex.Message;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

            }
        }
    }
}
