using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections;
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
                        if (string.IsNullOrWhiteSpace(cell1))
                        {
                            this.lblMsg.Text = "第" + rowCount.ToString() + "行档案号不能为空。";
                            hasError = true;
                            break;
                        }
                        string cell2 = ImportHelper.GetCellStringValue(row.GetCell(1));  //人员类别
                        string cell3 = ImportHelper.GetCellStringValue(row.GetCell(2));  //姓名
                        if (string.IsNullOrWhiteSpace(cell3))
                        {
                            this.lblMsg.Text = "第" + rowCount.ToString() + "行姓名不能为空。";
                            hasError = true;
                            break;
                        }
                        string cell4 = ImportHelper.GetCellStringValue(row.GetCell(3));  //结算部门
                        string cell5 = ImportHelper.GetCellStringValue(row.GetCell(4));  //性别
                        string cell6 = ImportHelper.GetCellStringValue(row.GetCell(5));  //民族
                        string cell7 = ImportHelper.GetCellStringValue(row.GetCell(6));  //出生日期
                        if (!string.IsNullOrWhiteSpace(cell7))
                        {
                            try
                            {
                                cell7 = row.GetCell(6).DateCellValue.ToString("yyyy-MM-dd");
                            }
                            catch (Exception)
                            {
                                this.lblMsg.Text = "第" + rowCount.ToString() + "行出生日期格式不正确。";
                                hasError = true;
                                break;
                            }
                        }
                        string cell8 = ImportHelper.GetCellStringValue(row.GetCell(7));  //入伍地点（户籍地）
                        string cell9 = ImportHelper.GetCellStringValue(row.GetCell(8));  //文化程度
                        string cell10 = ImportHelper.GetCellStringValue(row.GetCell(9));  //入伍前工龄
                        string cell11 = ImportHelper.GetCellStringValue(row.GetCell(10)); //工作时间
                        if (!string.IsNullOrWhiteSpace(cell11))
                        {
                            try
                            {
                                cell11 = row.GetCell(10).DateCellValue.ToString("yyyy-MM-dd");
                            }
                            catch (Exception)
                            {
                                this.lblMsg.Text = "第" + rowCount.ToString() + "行工作时间格式不正确。";
                                hasError = true;
                                break;
                            }
                        }
                        string cell12 = ImportHelper.GetCellStringValue(row.GetCell(11));  //入伍时间
                        if (!string.IsNullOrWhiteSpace(cell12))
                        {
                            try
                            {
                                cell12 = row.GetCell(11).DateCellValue.ToString("yyyy-MM-dd");
                            }
                            catch (Exception)
                            {
                                this.lblMsg.Text = "第" + rowCount.ToString() + "行入伍时间格式不正确。";
                                hasError = true;
                                break;
                            }
                        }
                        string cell13 = ImportHelper.GetCellStringValue(row.GetCell(12));  //护教龄
                        string cell14 = ImportHelper.GetCellStringValue(row.GetCell(13));  //职务等级
                        string cell15 = ImportHelper.GetCellStringValue(row.GetCell(14));  //军衔级别
                        string cell16 = ImportHelper.GetCellStringValue(row.GetCell(15));  //专业技术职务
                        string cell17 = ImportHelper.GetCellStringValue(row.GetCell(16));  //离退时间
                        if (!string.IsNullOrWhiteSpace(cell17))
                        {
                            try
                            {
                                cell17 = row.GetCell(16).DateCellValue.ToString("yyyy-MM-dd");
                            }
                            catch (Exception)
                            {
                                this.lblMsg.Text = "第" + rowCount.ToString() + "行离退时间格式不正确。";
                                hasError = true;
                                break;
                            }
                        }
                        string cell18 = ImportHelper.GetCellStringValue(row.GetCell(17));  //计发比例
                        string cell19 = ImportHelper.GetCellStringValue(row.GetCell(18));  //婚姻状况
                        string cell20 = ImportHelper.GetCellStringValue(row.GetCell(19));  //子女状况
                        string cell21 = ImportHelper.GetCellStringValue(row.GetCell(20));  //享受住房补贴状况
                        string cell22 = ImportHelper.GetCellStringValue(row.GetCell(21));  //伤残等级
                        string cell23 = ImportHelper.GetCellStringValue(row.GetCell(22));  //提干时间
                        if (!string.IsNullOrWhiteSpace(cell23))
                        {
                            try
                            {
                                cell23 = row.GetCell(22).DateCellValue.ToString("yyyy-MM-dd");
                            }
                            catch (Exception)
                            {
                                this.lblMsg.Text = "第" + rowCount.ToString() + "行提干时间格式不正确。";
                                hasError = true;
                                break;
                            }
                        }
                        string cell24 = ImportHelper.GetCellStringValue(row.GetCell(23));  //调入时间
                        if (!string.IsNullOrWhiteSpace(cell24))
                        {
                            try
                            {
                                cell24 = row.GetCell(23).DateCellValue.ToString("yyyy-MM-dd");
                            }
                            catch (Exception)
                            {
                                this.lblMsg.Text = "第" + rowCount.ToString() + "行调入时间格式不正确。";
                                hasError = true;
                                break;
                            }
                        }
                        string cell25 = ImportHelper.GetCellStringValue(row.GetCell(24));  //停供国防费时间
                        if (!string.IsNullOrWhiteSpace(cell25))
                        {
                            try
                            {
                                cell25 = row.GetCell(24).DateCellValue.ToString("yyyy-MM-dd");
                            }
                            catch (Exception)
                            {
                                this.lblMsg.Text = "第" + rowCount.ToString() + "行停供国防费时间格式不正确。";
                                hasError = true;
                                break;
                            }
                        }
                        string cell26 = ImportHelper.GetCellStringValue(row.GetCell(25));  //选改士官时间
                        if (!string.IsNullOrWhiteSpace(cell26))
                        {
                            try
                            {
                                cell26 = row.GetCell(25).DateCellValue.ToString("yyyy-MM-dd");
                            }
                            catch (Exception)
                            {
                                this.lblMsg.Text = "第" + rowCount.ToString() + "行选改士官时间格式不正确。";
                                hasError = true;
                                break;
                            }
                        }
                        string cell27 = ImportHelper.GetCellStringValue(row.GetCell(26));  //入伍登记表编号
                        string cell28 = ImportHelper.GetCellStringValue(row.GetCell(27));  //选改士官提干命令号
                        string cell29 = ImportHelper.GetCellStringValue(row.GetCell(28));  //部队驻地
                        string cell30 = ImportHelper.GetCellStringValue(row.GetCell(29));  //原单位
                        string cell31 = ImportHelper.GetCellStringValue(row.GetCell(30));  //供给介绍信编号
                        string cell32 = ImportHelper.GetCellStringValue(row.GetCell(31));  //公民身份证号
                        if (string.IsNullOrWhiteSpace(cell32))
                        {
                            this.lblMsg.Text = "第" + rowCount.ToString() + "行公民身份证号不能为空。";
                            hasError = true;
                            break;
                        }
                        string cell33 = ImportHelper.GetCellStringValue(row.GetCell(32));  //军人保障卡号
                        string cell34 = ImportHelper.GetCellStringValue(row.GetCell(33));  //干部编号
                        string cell35 = ImportHelper.GetCellStringValue(row.GetCell(34));  //人员来源
                        string cell36 = ImportHelper.GetCellStringValue(row.GetCell(35));  //储蓄账号
                        string cell37 = ImportHelper.GetCellStringValue(row.GetCell(36));  //院校学员
                        string cell38 = ImportHelper.GetCellStringValue(row.GetCell(37));  //学籍号
                        string cell39 = ImportHelper.GetCellStringValue(row.GetCell(38));  //备注
                        string cell40 = ImportHelper.GetCellStringValue(row.GetCell(39));  //不自动计算医保
                        string cell41 = ImportHelper.GetCellStringValue(row.GetCell(40));  //不自动计算税款
                        string cell42 = ImportHelper.GetCellStringValue(row.GetCell(41));  //不自动计算大病互助保险
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
                            dataTable.Rows[0][9] = cell9;
                            dataTable.Rows[0][10] = cell10;
                            dataTable.Rows[0][11] = cell11;
                            dataTable.Rows[0][12] = cell12;
                            dataTable.Rows[0][13] = cell13;
                            dataTable.Rows[0][14] = cell14;
                            dataTable.Rows[0][15] = cell15;
                            dataTable.Rows[0][16] = cell16;
                            dataTable.Rows[0][17] = cell17;
                            dataTable.Rows[0][18] = cell18;
                            dataTable.Rows[0][19] = cell19;
                            dataTable.Rows[0][20] = cell20;
                            dataTable.Rows[0][21] = cell21;
                            dataTable.Rows[0][22] = cell22;
                            dataTable.Rows[0][23] = cell23;
                            dataTable.Rows[0][24] = cell24;
                            dataTable.Rows[0][25] = cell25;
                            dataTable.Rows[0][26] = cell26;
                            dataTable.Rows[0][27] = cell27;
                            dataTable.Rows[0][28] = cell28;
                            dataTable.Rows[0][29] = cell29;
                            dataTable.Rows[0][30] = cell30;
                            dataTable.Rows[0][31] = cell31;
                            dataTable.Rows[0][32] = cell32;
                            dataTable.Rows[0][33] = cell33;
                            dataTable.Rows[0][34] = cell34;
                            dataTable.Rows[0][35] = cell35;
                            dataTable.Rows[0][36] = cell36;
                            dataTable.Rows[0][37] = cell37;
                            dataTable.Rows[0][38] = cell38;
                            dataTable.Rows[0][39] = cell39;
                            dataTable.Rows[0][40] = cell40;
                            dataTable.Rows[0][41] = cell41;
                            dataTable.Rows[0][42] = cell42;

                            adapter.Update(dataTable);
                        }
                        else
                        {
                            int idCount = (int)adapter.SelectCountBy公民身份证号(cell32);
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
                                            cell8,
                                            cell9,
                                            cell10,
                                            cell11,
                                            cell12,
                                            cell13,
                                            cell14,
                                            cell15,
                                            cell16,
                                            cell17,
                                            cell18,
                                            cell19,
                                            cell20,
                                            cell21,
                                            cell22,
                                            cell23,
                                            cell24,
                                            cell25,
                                            cell26,
                                            cell27,
                                            cell28,
                                            cell29,
                                            cell30,
                                            cell31,
                                            cell32,
                                            cell33,
                                            cell34,
                                            cell35,
                                            cell36,
                                            cell37,
                                            cell38,
                                            cell39,
                                            cell40,
                                            cell41,
                                            cell42);
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
    }
}
