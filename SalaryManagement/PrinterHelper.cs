using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace SalaryManagement.Utility
{
    public class PrinterHelper
    {
        public static void PrintOrder(string fileUrl, string printer)
        {
            KillProcess("EXCEL");
            Microsoft.Office.Interop.Excel.Application myExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook myBook = null;
            Microsoft.Office.Interop.Excel.Worksheet mySheet1 = null;

            Object missing = System.Reflection.Missing.Value;
            Object defaultPrint = missing;

            //string print = Settings.Default.DefaultPrintName1;
            if (printer != null && printer != string.Empty)
            {
                defaultPrint = printer;
            }

            try
            {

                myBook = myExcel.Workbooks.Open(fileUrl, missing, missing, missing, missing,
                    missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                //handle sheets
                mySheet1 = (Microsoft.Office.Interop.Excel.Worksheet)myBook.Sheets[1];
                mySheet1.Columns.AutoFit();
                mySheet1.PrintOut(missing, missing, missing, missing, defaultPrint, missing, missing, missing);
            }
            catch (Exception e) 

            {
                string errorMsg = "打印失败,重打请按CTRL+P!错误信息:" + e.Message;
                MessageBox.Show(errorMsg, "打印失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (myBook != null)
                {
                    myBook.Close(false, missing, missing);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(myBook);
                }
                if (mySheet1 != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mySheet1);
                }
                myBook = null;
                mySheet1 = null;
                myExcel.Quit();
                GC.Collect();
            }
        }

        private static void KillProcess(string processName)
        {
            //获得进程对象，以用来操作   
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            //得到所有打开的进程    
            try
            {
                //获得需要杀死的进程名   
                foreach (Process thisproc in Process.GetProcessesByName(processName))
                {
                    //立即杀死进程   
                    thisproc.Kill();
                }
            }
            catch (Exception Exc)
            {
                //throw new Exception("", Exc);
            }
        }
    }
}
