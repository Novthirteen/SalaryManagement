﻿using System;
using System.Collections;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace SalaryManagement.Utility
{
    public static class ImportHelper
    {
        /// <summary>
        /// 跳过N行
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        public static void JumpRows(IEnumerator rows, int count)
        {
            for (int i = 0; i < count; i++)
            {
                rows.MoveNext();
            }
        }


        public static bool CheckValidDataRow(HSSFRow row, int startColIndex, int endColIndex)
        {
            for (int i = startColIndex; i < endColIndex; i++)
            {
                ICell cell = row.GetCell(i);
                if (cell != null && cell.CellType != NPOI.SS.UserModel.CellType.BLANK)
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetCellStringValue(ICell cell)
        {
            string strValue = null;
            if (cell != null)
            {
                if (cell.CellType == CellType.STRING)
                {
                    strValue = cell.StringCellValue;
                }
                else if (cell.CellType == CellType.NUMERIC)
                {
                    strValue = cell.NumericCellValue.ToString("0.########");
                }
                else if (cell.CellType == CellType.BOOLEAN)
                {
                    strValue = cell.BooleanCellValue.ToString();
                }
                else if (cell.CellType == CellType.FORMULA)
                {
                    if (cell.CachedFormulaResultType == CellType.STRING)
                    {
                        strValue = cell.StringCellValue;
                    }
                    else if (cell.CachedFormulaResultType == CellType.NUMERIC)
                    {
                        strValue = cell.NumericCellValue.ToString("0.########");
                    }
                    else if (cell.CachedFormulaResultType == CellType.BOOLEAN)
                    {
                        strValue = cell.NumericCellValue.ToString();
                    }
                }
            }
            if (strValue != null)
            {
                strValue = strValue.Trim();
            }
            strValue = strValue == string.Empty ? null : strValue;
            return strValue;
        }

        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
