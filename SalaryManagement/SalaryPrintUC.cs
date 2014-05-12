using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using SalaryManagement.Utility;

namespace SalaryManagement
{
    public partial class SalaryPrintUC : UserControl
    {
        public SalaryPrintUC()
        {
            InitializeComponent();
        }

        private void btPrint_Click(object sender, EventArgs e)
        {
            this.lblMsg.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(tbId.Text))
            {
                this.lblMsg.Text = "身份证号不能为空。";
                return;
            }

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

            OleDbConnection conn = new OleDbConnection(Properties.Settings.Default.ConnectionString);
            try
            {
                SalaryManagementDataSetTableAdapters.StaffTableAdapter stAdapter = new SalaryManagementDataSetTableAdapters.StaffTableAdapter();
                SalaryManagementDataSet.StaffDataTable sDataTable = stAdapter.GetDataBy公民身份证号(tbId.Text);
                if (sDataTable.Rows.Count == 0)
                {
                    this.lblMsg.Text = "身份证号" + tbId.Text + "不存在。";
                    return;
                }

                SalaryManagementDataSetTableAdapters.SalarySettingTableAdapter sstAdapter = new SalaryManagementDataSetTableAdapters.SalarySettingTableAdapter();
                SalaryManagementDataSet.SalarySettingDataTable ssDataTable = sstAdapter.GetData();
                IList<SalarySetting> salarySettingList = new List<SalarySetting>();
                foreach (DataRow dr in ssDataTable)
                {
                    SalarySetting ss = new SalarySetting();
                    ss.Item = (string)dr[1];
                    ss.ItemType = (string)dr[2];
                    ss.Seq = (int)dr[4];
                    salarySettingList.Add(ss);
                }

                OleDbDataAdapter dbAdapter = new OleDbDataAdapter("select a.* from Salary as a inner join Staff as b on a.StaffId = b.Id where a.YM = '" + dtYM.ToString("yyyyMM") + "' and b.公民身份证号 = '" + tbId.Text + "'", conn);
                DataSet dataSet = new DataSet();
                conn.Open();

                dbAdapter.Fill(dataSet);

                if (dataSet.Tables[0].Rows.Count == 0)
                {
                    this.lblMsg.Text = "身份证号" + tbId.Text + "月份" + dtYM.ToString("yyyyMM") + "的工资数据不存在。";
                    return;
                }

                IList<PrintCell> printCellList = new List<PrintCell>();
                for (int i = 3; i < dataSet.Tables[0].Columns.Count; i++)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        if (dr[i] != null && (decimal)dr[i] != 0)
                        {
                            PrintCell pc = new PrintCell();
                            pc.StaffId = (int)sDataTable.Rows[0][0];
                            pc.YM = dtYM.ToString("yyyyMM");
                            pc.Item = ssDataTable.Where(ss => ss.列名 == dataSet.Tables[0].Columns[i].ColumnName).Single().项目;
                            pc.ItemType = ssDataTable.Where(ss => ss.列名 == dataSet.Tables[0].Columns[i].ColumnName).Single().类型;
                            pc.Amount = (decimal)dr[i];

                            printCellList.Add(pc);
                        }
                    }
                }

                string tempFileName = System.IO.Path.GetTempFileName();
                FileStream fileStream = new FileStream(tempFileName, FileMode.Create);

                int rowIndex = 0;
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                ISheet sheet = hssfworkbook.CreateSheet("Sheet1");

                IRow rowHead = sheet.CreateRow(rowIndex);
                rowHead.HeightInPoints = 30;//行高
                ICell cell = rowHead.CreateCell(0);
                CellRangeAddress cellRangeAddress = new CellRangeAddress(0, 0, 0, 8);
                sheet.AddMergedRegion(cellRangeAddress);
                cell.SetCellValue(dtYM.ToString("yyyy") + "年" + dtYM.ToString("MM") + "月份工资单");
                ICellStyle style = hssfworkbook.CreateCellStyle();
                IFont font = hssfworkbook.CreateFont();
                font.Boldweight = (short)FontBoldWeight.BOLD;
                font.FontHeightInPoints = (short)16;
                style.SetFont(font);
                style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
                cell.CellStyle = style;

                #region 打印人员基本信息
                #region 头
                rowIndex++;
                rowHead = sheet.CreateRow(rowIndex);
                rowHead.CreateCell(0).SetCellValue("编号");
                rowHead.CreateCell(1).SetCellValue("人员类别");
                rowHead.CreateCell(2).SetCellValue("姓 名");
                rowHead.CreateCell(3).SetCellValue("结算部门");
                rowHead.CreateCell(4).SetCellValue("职务等级");
                rowHead.CreateCell(5).SetCellValue("军衔级别");
                rowHead.CreateCell(6).SetCellValue("储蓄账号");
                #endregion

                #region 内容
                rowIndex++;
                IRow rowContent = sheet.CreateRow(rowIndex);
                rowContent.CreateCell(0).SetCellValue("1");
                rowContent.CreateCell(1).SetCellValue((string)sDataTable.Rows[0][2]);
                rowContent.CreateCell(2).SetCellValue((string)sDataTable.Rows[0][3]);
                rowContent.CreateCell(3).SetCellValue((string)sDataTable.Rows[0][4]);
                rowContent.CreateCell(4).SetCellValue((string)sDataTable.Rows[0][14]);
                rowContent.CreateCell(5).SetCellValue((string)sDataTable.Rows[0][15]);
                rowContent.CreateCell(6).SetCellValue((string)sDataTable.Rows[0][36]);
                cellRangeAddress = new CellRangeAddress(2, 2, 6, 8);
                sheet.AddMergedRegion(cellRangeAddress);
                #endregion
                #endregion

                #region 打印应发
                IList<string> printHeadList = new List<string>();
                foreach (var ss in (from pc in printCellList
                                    join ss in salarySettingList on pc.Item equals ss.Item into result
                                    from r in result
                                    where r.ItemType == "应发"
                                    select new
                                    {
                                        r.Item,
                                        r.ItemType,
                                        r.Seq,
                                    }).Distinct().OrderByDescending(ss => ss.ItemType).ThenBy(ss => ss.Seq))
                {
                    printHeadList.Add(ss.Item);
                }

                int skipIndex = 0;
                IList<string> thisPrintHeadList = printHeadList.Skip(skipIndex).Take(7).ToList();
                decimal subTotal = 0;
                decimal grandTotal = 0;
                while (true)
                {
                    rowIndex++;
                    rowHead = sheet.CreateRow(rowIndex);
                    rowIndex++;
                    rowContent = sheet.CreateRow(rowIndex);

                    int i = 0;
                    for (; i < thisPrintHeadList.Count; i++)
                    {
                        PrintCell printCell = printCellList.Where(pc => pc.Item == thisPrintHeadList[i]).SingleOrDefault();
                        rowHead.CreateCell(i).SetCellValue(thisPrintHeadList[i]);
                        rowContent.CreateCell(i).SetCellValue(printCell != null ? printCell.Amount.ToString("#.##") : string.Empty);
                        subTotal += printCell != null ? printCell.Amount : 0;
                        grandTotal += printCell != null ? printCell.Amount : 0;
                    }

                    skipIndex += thisPrintHeadList.Count();
                    thisPrintHeadList = printHeadList.Skip(skipIndex).Take(7).ToList();

                    if (thisPrintHeadList == null || thisPrintHeadList.Count == 0)
                    {
                        style = hssfworkbook.CreateCellStyle();
                        font = hssfworkbook.CreateFont();
                        font.Boldweight = (short)FontBoldWeight.BOLD;
                        font.FontHeightInPoints = (short)12;
                        style.SetFont(font);
                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;

                        cell = rowHead.CreateCell(i);
                        cell.SetCellValue("应发");
                        cell.CellStyle = style;

                        cell = rowContent.CreateCell(i);
                        cell.SetCellValue(subTotal.ToString("#.##"));
                        cell.CellStyle = style;
                        break;
                    }
                }
                #endregion

                #region 打印扣发
                printHeadList = new List<string>();
                foreach (var ss in (from pc in printCellList
                                    join ss in salarySettingList on pc.Item equals ss.Item into result
                                    from r in result
                                    where r.ItemType == "扣发"
                                    select new
                                    {
                                        r.Item,
                                        r.ItemType,
                                        r.Seq,
                                    }).Distinct().OrderByDescending(ss => ss.ItemType).ThenBy(ss => ss.Seq))
                {
                    printHeadList.Add(ss.Item);
                }

                skipIndex = 0;
                thisPrintHeadList = printHeadList.Skip(skipIndex).Take(7).ToList();
                subTotal = 0;
                while (true)
                {
                    rowIndex++;
                    rowHead = sheet.CreateRow(rowIndex);
                    rowIndex++;
                    rowContent = sheet.CreateRow(rowIndex);

                    int i = 0;
                    for (; i < thisPrintHeadList.Count; i++)
                    {
                        PrintCell printCell = printCellList.Where(pc => pc.Item == thisPrintHeadList[i]).SingleOrDefault();
                        rowHead.CreateCell(i).SetCellValue(thisPrintHeadList[i]);
                        rowContent.CreateCell(i).SetCellValue(printCell != null ? printCell.Amount.ToString("#.##") : string.Empty);
                        subTotal += printCell != null ? printCell.Amount : 0;
                        grandTotal -= printCell != null ? printCell.Amount : 0;
                    }

                    skipIndex += thisPrintHeadList.Count();
                    thisPrintHeadList = printHeadList.Skip(skipIndex).Take(7).ToList();

                    if (thisPrintHeadList == null || thisPrintHeadList.Count == 0)
                    {
                        cell = rowHead.CreateCell(i);
                        cell.SetCellValue("扣发");
                        cell.CellStyle = style;

                        cell = rowContent.CreateCell(i);
                        cell.SetCellValue(subTotal.ToString("#.##"));
                        cell.CellStyle = style;
                        break;
                    }
                }
                #endregion

                #region 打印汇总
                rowIndex++;
                rowHead = sheet.CreateRow(rowIndex);
                rowIndex++;
                rowContent = sheet.CreateRow(rowIndex);
                cell = rowHead.CreateCell(0);
                cell.SetCellValue("实发工资");
                cell.CellStyle = style;

                cell = rowContent.CreateCell(0);
                cell.SetCellValue(grandTotal.ToString("#.##"));
                cell.CellStyle = style;
                #endregion

                hssfworkbook.Write(fileStream);
                fileStream.Close();

                PrinterHelper.PrintOrder(tempFileName, null);
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

    public class SalarySetting
    {
        public string Item { get; set; }
        public string ItemType { get; set; }
        public int Seq { get; set; }
    }

    public class PrintCell
    {
        public int StaffId { get; set; }
        public string YM { get; set; }
        public string Item { get; set; }
        public string ItemType { get; set; }
        public decimal Amount { get; set; }
    }
}
