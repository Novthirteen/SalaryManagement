using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SalaryManagement.Utility;

namespace SalaryManagement
{
    public partial class StaffUC : UserControl
    {
        public StaffUC()
        {
            InitializeComponent();
            InitCBDepartment();
        }

        private void InitCBDepartment()
        {
            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnectionString);
            OleDbDataAdapter dbAdapter = new OleDbDataAdapter("select distinct 结算部门 from Staff", conn);
            DataSet dataSet = new DataSet();
            try
            {
                conn.Open();
                dbAdapter.Fill(dataSet);
                conn.Close();
            }
            catch (Exception)
            {
                this.lblMsg.Text = "连接数据库失败。";
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }

            IList<string> result = new List<string>();
            result.Add(string.Empty);
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                result.Add(row[0].ToString());
            }
            this.cbDepartment.DataSource = result;
        }

        private void btSearch_Click(object sender, System.EventArgs e)
        {
            doSearch();
        }

        private void doSearch()
        {
            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnectionString);
            string selectStatement = "select * from Staff where 1 = 1";
            if (!string.IsNullOrWhiteSpace(tbCode.Text))
            {
                selectStatement += " and 档案号 = '" + tbCode.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(tbName.Text))
            {
                selectStatement += " and 姓名 = '" + tbName.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(tbId.Text))
            {
                selectStatement += " and 公民身份证号 = '" + tbId.Text + "'";
            }
            if (!string.IsNullOrWhiteSpace(cbDepartment.SelectedValue.ToString()))
            {
                selectStatement += " and 结算部门 = '" + cbDepartment.SelectedValue.ToString() + "'";
            }
            OleDbDataAdapter dbAdapter = new OleDbDataAdapter(selectStatement, conn);
            DataSet dataSet = new DataSet();
            try
            {
                conn.Open();
                dbAdapter.Fill(dataSet);
            }
            catch (Exception)
            {
                this.lblMsg.Text = "连接数据库失败。";
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }

            this.gvResult.DataSource = dataSet.Tables[0];
            this.gvResult.Columns[0].Visible = false;
            this.gvResult.Columns[1].ReadOnly = true;
            this.gvResult.Visible = true;
        }

        private void btUpload_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "xls文件 (*.xls)|*.xls|xlsx文件 (*.xlsx)|*.xlsx|所有文件 (*.*)|*.*"; // file types, that will be allowed to upload
            dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                doUpload(dialog.FileName);
            }
        }

        private void doUpload(string fileName)
        {

            using (FileStream stream = File.Open(fileName, FileMode.Open))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rows = sheet.GetRowEnumerator();
                rows.MoveNext();

                SalaryManagementDataSetTableAdapters.StaffTableAdapter adapter = new SalaryManagementDataSetTableAdapters.StaffTableAdapter();
                bool hasError = false;

                try
                {
                    int rowCount = 0;
                    while (rows.MoveNext())
                    {
                        rowCount++;
                        HSSFRow row = (HSSFRow)rows.Current;
                        if (!ImportHelper.CheckValidDataRow(row, 1, 42))
                        {
                            break;//边界
                        }

                        #region 读取数据
                        string cell1 = ImportHelper.GetCellStringValue(row.GetCell(0));  //档案号
                        string cell2 = ImportHelper.GetCellStringValue(row.GetCell(1));  //人员类别
                        string cell3 = ImportHelper.GetCellStringValue(row.GetCell(2));  //姓名
                        if (string.IsNullOrWhiteSpace(cell3))
                        {
                            this.lblMsg.Text = "第" + rowCount.ToString() + "行姓名不能为空。";
                            hasError = true;
                            break;
                        }
                        string cell4 = ImportHelper.GetCellStringValue(row.GetCell(3));  //结算部门
                        string cell5 = ImportHelper.GetCellStringValue(row.GetCell(4));  //职务等级
                        string cell6 = ImportHelper.GetCellStringValue(row.GetCell(5));  //军衔级别
                        string cell7 = ImportHelper.GetCellStringValue(row.GetCell(6));  //公民身份证号
                        string cell8 = ImportHelper.GetCellStringValue(row.GetCell(7));  //储蓄账号
                        #endregion

                        SalaryManagementDataSet.StaffDataTable dataTable = new SalaryManagementDataSet.StaffDataTable();
                        adapter.FillBy档案号(dataTable, cell1);


                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataTable.Rows[0][2] = cell2;
                            dataTable.Rows[0][3] = cell3;
                            dataTable.Rows[0][4] = cell4;
                            dataTable.Rows[0][5] = cell5;
                            dataTable.Rows[0][6] = cell6;
                            dataTable.Rows[0][7] = cell7;
                            dataTable.Rows[0][8] = cell8;

                            adapter.Update(dataTable);
                        }
                        else
                        {
                            int idCount = (int)adapter.SelectCountBy公民身份证号(cell7);
                            if (idCount > 0)
                            {
                                this.lblMsg.Text = "第" + rowCount.ToString() + "行公民身份证号已经存在。";
                                hasError = true;
                                break;
                            }

                            adapter.Insert(cell1,
                                            cell2,
                                            cell3,
                                            cell4,
                                            cell5,
                                            cell6,
                                            cell7,
                                            cell8);
                        }
                    }

                    if (!hasError)
                    {
                        this.lblMsg.Text = "员工基本信息导入成功。";
                    }
                }
                catch (Exception)
                {
                    this.lblMsg.Text = "连接数据库失败。";
                }
                finally
                {
                    adapter.Dispose();
                    InitCBDepartment();
                    doSearch();
                }
            }
        }

        private void OnCellEndEidt(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView gv = ((DataGridView)sender);
            DataGridViewColumn col = gv.Columns[e.ColumnIndex];
            DataGridViewCell cell1 = gv.Rows[e.RowIndex].Cells[0];
            DataGridViewCell cell2 = gv.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (!!string.IsNullOrWhiteSpace(cell1.Value.ToString()))
            {
                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnectionString);
                OleDbCommand command = new OleDbCommand();
                command.CommandText = "update Staff set " + col.Name + " = '" + cell2.Value + "' where Id = " + cell1.Value.ToString();
                command.Connection = conn;

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception)
                {
                    this.lblMsg.Text = "连接数据库失败。";
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

                InitCBDepartment();
            }
        }

        private void OnUserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dlgRes = MessageBox.Show(
                                    "你确认要删除这条记录吗？",
                                    "删除确认",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question);

            if (dlgRes == DialogResult.Yes)
            {
                DataGridView gv = ((DataGridView)sender);
                DataGridViewCell cell1 = gv.Rows[e.Row.Index].Cells[0];
                DataGridViewCell cell2 = gv.Rows[e.Row.Index].Cells[1];
                OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnectionString);
                OleDbCommand command = new OleDbCommand();
                command.CommandText = "delete from Staff where Id = " + cell1.Value.ToString();
                command.Connection = conn;

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();

                    this.lblMsg.Text = "档案号" + cell2.Value.ToString() + "删除成功。";
                }
                catch (Exception)
                {
                    this.lblMsg.Text = "连接数据库失败。";
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

                InitCBDepartment();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void OnUserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            doSearch();
        }

        private void lldownLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebClient myWebClient = new WebClient();
            myWebClient.DownloadFile("人员基本信息导入模板.xls", "人员基本信息导入模板.xls");		
        }
    }
}
