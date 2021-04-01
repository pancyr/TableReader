using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Office.Interop.Excel;

namespace TableReader.Core
{
    public class Page
    {
        public Page(Worksheet sheet, Styles styles = null)
        {
            this._excelSheet = sheet;
            this._name = sheet.Name;
            this.Styles = styles;
        }

        protected Worksheet _excelSheet; // лист книги

        private string _name;  // поле для хранения Имени листа
        public string Name => _name;     // своиство Имя

        public bool SetName(string name)
        {
            this._name = name;
            this._excelSheet.Name = this.Name;
            return true;
        }

        public int TotalRows { get; set; }
        public int TotalColumns { get; set; }
        public int CurrentRow { get; set; }

        public readonly Styles Styles;

        public virtual void InitSheetParams()
        {
            this.TotalRows = _excelSheet.UsedRange.Rows.Count;
            this.TotalColumns = _excelSheet.UsedRange.Columns.Count;
        }

        #region Функции чтения ячеек

        public int FindInColumn(int columnNum, String param, int startRow)
        {
            if (columnNum > 0 & startRow > 0)
            {
                object cell_1, cell_2;
                cell_1 = _excelSheet.Cells[startRow, columnNum];
                cell_2 = _excelSheet.Cells[this.TotalRows, columnNum];
                Range rngDiapos = ((Range)_excelSheet.get_Range(cell_1, cell_2));

                foreach (object theCell in rngDiapos.Cells)
                    if (((Range)theCell).Text.ToString().Trim().ToUpper() == param.Trim().ToUpper())
                        return ((Range)theCell).Row;
            }
            return 0;
        }

        public int FindInRow(int rowNum, String param, int startColumn)
        {
            if (rowNum > 0 & startColumn > 0)
            {
                object cell_1, cell_2;
                cell_1 = _excelSheet.Cells[rowNum, startColumn];
                cell_2 = _excelSheet.Cells[rowNum, this.TotalColumns];
                Range rngDiapos = ((Range)_excelSheet.get_Range(cell_1, cell_2));

                foreach (object theCell in rngDiapos.Cells)
                    if (((Range)theCell).Text.ToString().Trim().ToUpper() == param.Trim().ToUpper())
                        return ((Range)theCell).Column;
            }
            return 0;
        }

        public Range GetCell(object row, object column)
        {
            return (Range)_excelSheet.Cells[row, column];
        }

        public string ReadValue(int column) => ReadValue(this.CurrentRow, column);
        public string ReadText(int column) => ReadText(this.CurrentRow, column);

        public string ReadValue(int row, int column) => GetCell(row, column).Value2 == null ? String.Empty : GetCell(row, column).Value2.ToString().Trim();
        public string ReadText(int row, int column) => GetCell(row, column).Text == null ? String.Empty : GetCell(row, column).Text.ToString().Trim();

        public bool IsHiddenRow(int num)
        {
            Range range = ((Range)_excelSheet.Rows[num, Type.Missing]);
            return Boolean.Parse(range.Hidden.ToString());
        }

        public bool IsHiddenColumn(int num)
        {
            Range range = ((Range)_excelSheet.Columns[num, Type.Missing]);
            return Boolean.Parse(range.Hidden.ToString());
        }

        public Boolean IsCellBorderTop(int row, int cell)
        {
            Borders allBorders = ((Range)_excelSheet.Cells[row, cell]).Borders;
            Border theBorder = (Border)allBorders.get_Item(XlBordersIndex.xlEdgeTop);
            return ((XlLineStyle)theBorder.LineStyle) != XlLineStyle.xlLineStyleNone;
        }

        public Boolean IsCellBorderButton(int row, int cell)
        {
            Borders allBorders = ((Range)_excelSheet.Cells[row, cell]).Borders;
            Border theBorder = (Border)allBorders.get_Item(XlBordersIndex.xlEdgeBottom);
            return ((XlLineStyle)theBorder.LineStyle) != XlLineStyle.xlLineStyleNone;
        }

        public Boolean IsCellBorderLeft(int row, int cell)
        {
            Borders allBorders = ((Range)_excelSheet.Cells[row, cell]).Borders;
            Border theBorder = (Border)allBorders.get_Item(XlBordersIndex.xlEdgeLeft);
            return ((XlLineStyle)theBorder.LineStyle) != XlLineStyle.xlLineStyleNone;
        }

        public Boolean IsCellBorderRight(int row, int cell)
        {
            Borders allBorders = ((Range)_excelSheet.Cells[row, cell]).Borders;
            Border theBorder = (Border)allBorders.get_Item(XlBordersIndex.xlEdgeRight);
            return ((XlLineStyle)theBorder.LineStyle) != XlLineStyle.xlLineStyleNone;
        }

        public Boolean IsVerticalBorderInLine(int cellBegin, int cellEnd)
        {
            object cell_1, cell_2;
            cell_1 = _excelSheet.Cells[CurrentRow, cellBegin];
            cell_2 = _excelSheet.Cells[CurrentRow, cellEnd];
            Borders allBorders = ((Range)_excelSheet.get_Range(cell_1, cell_2)).Borders;
            Border theBorder = (Border)allBorders.get_Item(XlBordersIndex.xlInsideVertical);
            return ((XlLineStyle)theBorder.LineStyle) != XlLineStyle.xlLineStyleNone;
        }

        public Boolean IsHorizontalBorderInColumn(int rowBegin, int rowEnd, int cell)
        {
            object cell_1, cell_2;
            cell_1 = _excelSheet.Cells[rowBegin, cell];
            cell_2 = _excelSheet.Cells[rowEnd, cell];
            Borders allBorders = ((Range)_excelSheet.get_Range(cell_1, cell_2)).Borders;
            Border theBorder = (Border)allBorders.get_Item(XlBordersIndex.xlInsideHorizontal);
            return ((XlLineStyle)theBorder.LineStyle) != XlLineStyle.xlLineStyleNone;
        }

        public Boolean IsAroundBorderLine(int row, int cellBegin, int cellEnd)
        {
            object cell_1, cell_2;
            bool isTop, isButton, isLeft, isRight;
            cell_1 = _excelSheet.Cells[row, cellBegin];
            cell_2 = _excelSheet.Cells[row, cellEnd];
            Borders allBorders = ((Range)_excelSheet.get_Range(cell_1, cell_2)).Borders;
            isTop = ((XlLineStyle)allBorders[XlBordersIndex.xlEdgeTop].LineStyle)
                != XlLineStyle.xlLineStyleNone;
            isButton = ((XlLineStyle)allBorders[XlBordersIndex.xlEdgeBottom].LineStyle)
                != XlLineStyle.xlLineStyleNone;
            isLeft = ((XlLineStyle)allBorders[XlBordersIndex.xlEdgeLeft].LineStyle)
                != XlLineStyle.xlLineStyleNone;
            isRight = ((XlLineStyle)allBorders[XlBordersIndex.xlEdgeRight].LineStyle)
                != XlLineStyle.xlLineStyleNone;
            return isTop & isButton & isLeft & isRight;
        }

        public Boolean IsOneRow(int row, int column)
        {
            Range theCell = (Range)_excelSheet.Cells[row, column];
            return theCell.MergeArea.Rows.Count == 1;
        }

        public Boolean IsOneColumn(int row, int column)
        {
            Range theCell = (Range)_excelSheet.Cells[row, column];
            return theCell.MergeArea.Columns.Count == 1;
        }

        public int MergeAreaRow(int row, int column)
        {
            Range theCell = (Range)_excelSheet.Cells[row, column];
            return theCell.MergeArea.Row;
        }

        public int MergeAreaColumn(int row, int column)
        {
            Range theCell = (Range)_excelSheet.Cells[row, column];
            return theCell.MergeArea.Column;
        }

        public int CountMargeRows(int row, int column)
        {
            Range theCell = (Range)_excelSheet.Cells[row, column];
            return theCell.MergeArea.Rows.Count;
        }

        public int CountMargeColumns(int column)
        {
            Range theCell = (Range)_excelSheet.Cells[CurrentRow, column];
            return theCell.MergeArea.Columns.Count;
        }

        public bool IsBoldFont(int row, int column)
        {
            if (((Range)_excelSheet.Cells[row, column]).Font.Bold != DBNull.Value)
                return (bool)((Range)_excelSheet.Cells[row, column]).Font.Bold;
            return false;
        }

        public bool IsItalicFont(int row, int column)
        {
            if (((Range)_excelSheet.Cells[row, column]).Font.Italic != DBNull.Value)
                return (bool)((Range)_excelSheet.Cells[row, column]).Font.Italic;
            return false;
        }

        public bool IsUnderlineFont(int row, int column)
        {
            return ((int)((Range)_excelSheet.Cells[row, column]).Font.Underline) > 0;
        }

        public int GetRowOutlineLevel(int row)
        {
            string sDiapos = String.Format("{0}:{1}", row, row);
            string sResult = ((Range)_excelSheet.Rows[sDiapos, Type.Missing]).OutlineLevel.ToString();
            if (sResult == String.Empty)
                return 0;
            return Int32.Parse(sResult);
        }

        public int GetColumnOutlineLevel(int column)
        {
            string sDiapos = String.Format("{0}:{1}", column, column);
            string sResult = ((Range)_excelSheet.Columns[Type.Missing, sDiapos]).OutlineLevel.ToString();
            if (sResult == String.Empty)
                return 0;
            return Int32.Parse(sResult);
        }

        public bool IsEndPosition() => CurrentRow > TotalRows;

        #endregion

        #region Функции редактирования ячеек

        public void OutputCell(int row, int column, string value) => GetCell(row, column).Value2 = value;

        public void MakeCellBold(int row, int column) => GetCell(row, column).Font.Bold = true;
        public void MakeCellItalic(int row, int column) => GetCell(row, column).Font.Italic = true;
        public void MakeCellUnderline(int row, int column) => GetCell(row, column).Font.Underline = true;

        public void CentrateCell(int column) => CentrateCell(this.CurrentRow, column);
        public void MergeColumns(int columnBegin, int columnEnd) => MergeColumns(this.CurrentRow, columnBegin, columnEnd);

        public void AutoFitColumns() => _excelSheet.UsedRange.Columns.AutoFit();

        public void ApplyStyle()
        {
            const String STYLE_NAME = "Обычный";
            Style mainStyle = this.Styles[STYLE_NAME];
            mainStyle.VerticalAlignment = XlVAlign.xlVAlignCenter;
            mainStyle.HorizontalAlignment = XlHAlign.xlHAlignCenter;
        }

        public void CentrateCell(int row, int column)
        {
            Range cell = _excelSheet.Cells[row, column];
            cell.VerticalAlignment = XlVAlign.xlVAlignCenter;
            cell.HorizontalAlignment = XlHAlign.xlHAlignCenter;
        }

        public void MergeRows(int columnNum, int rowBegin, int rowEnd)
        {
            object cell_1, cell_2;
            cell_1 = _excelSheet.Cells[rowBegin, columnNum];
            cell_2 = _excelSheet.Cells[rowEnd, columnNum];
            ((Range)_excelSheet.get_Range(cell_1, cell_2)).Merge(Type.Missing);
        }

        public void MergeColumns(int rowNum, int columnBegin, int columnEnd)
        {
            object cell_1, cell_2;
            cell_1 = _excelSheet.Cells[rowNum, columnBegin];
            cell_2 = _excelSheet.Cells[rowNum, columnEnd];
            ((Range)_excelSheet.get_Range(cell_1, cell_2)).Merge(Type.Missing);
        }

        public void LevelRowsHeight(int rowBegin, int rowEnd)
        {
            object cell_1, cell_2;
            cell_1 = _excelSheet.Cells[rowBegin, 1];
            cell_2 = _excelSheet.Cells[rowEnd, 1];
            Range range = ((Range)_excelSheet.get_Range(cell_1, cell_2));
            double totalHeight = (double)range.Height;
            double singleHeight = totalHeight / range.Rows.Count;
            foreach (Object row in range.Rows)
                ((Range)row).EntireRow.RowHeight = singleHeight;
        }

        public void MakeBorderArround(int rowBegin, int columnBegin, int rowEnd, int columnEnd, XlBorderWeight weight)
        {
            object cell_1, cell_2;
            cell_1 = _excelSheet.Cells[rowBegin, columnBegin];
            cell_2 = _excelSheet.Cells[rowEnd, columnEnd];
            Range range = ((Range)_excelSheet.get_Range(cell_1, cell_2));
            ((Range)range).BorderAround(XlLineStyle.xlContinuous, weight,
                    XlColorIndex.xlColorIndexAutomatic, Type.Missing);
        }

        public void MakeBorderCell(int rowBegin, int columnBegin, int rowEnd, int columnEnd, XlBorderWeight weight)
        {
            object cell_1, cell_2;
            cell_1 = _excelSheet.Cells[rowBegin, columnBegin];
            cell_2 = _excelSheet.Cells[rowEnd, columnEnd];
            Range range = ((Range)_excelSheet.get_Range(cell_1, cell_2));
            foreach (object cell in range.Cells)
                ((Range)cell).BorderAround(XlLineStyle.xlContinuous, weight,
                    XlColorIndex.xlColorIndexAutomatic, Type.Missing);
        }

        public void MakeGroup(int posFirst, int posLast)
        {
            string sDiapos = String.Format("{0}:{1}", posFirst, posLast);
            Range rng = (Range)_excelSheet.Rows[sDiapos, Type.Missing];
            rng.Group(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }

        public void SetRowHeight(int rowNum, int value)
        {
            object cell_1, cell_2;
            cell_1 = _excelSheet.Cells[rowNum, 1];
            cell_2 = _excelSheet.Cells[rowNum, 1];
            ((Range)_excelSheet.get_Range(cell_1, cell_2)).EntireRow.RowHeight = value;
        }

        public void SetTextSize(int rowBegin, int columnBegin, int rowEnd, int columnEnd, int size)
        {
            object cell_1, cell_2;
            cell_1 = _excelSheet.Cells[rowBegin, columnBegin];
            cell_2 = _excelSheet.Cells[rowEnd, columnEnd];
            ((Range)_excelSheet.get_Range(cell_1, cell_2)).Font.Size = size;
        }

        public void SetTextSize(int row, int column, int size)
        {
            SetTextSize(row, column, row, column, size);
        }

        public void MakeGroupRows(int rowBegin, int rowEnd)
        {
            string sDiapos = String.Format("{0}:{1}", rowBegin, rowEnd);
            Range rng = (Range)_excelSheet.Rows[sDiapos, Type.Missing];
            rng.Group(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }

        public void AutoFit_excelSheet(int rowBegin, int columnBegin, int rowEnd, int columnEnd)
        {
            object cell_1, cell_2;
            cell_1 = _excelSheet.Cells[rowBegin, columnBegin];
            cell_2 = _excelSheet.Cells[rowEnd, columnEnd];
            Range range = ((Range)_excelSheet.get_Range(cell_1, cell_2));
            range.Columns.AutoFit();
            range.Rows.AutoFit();
        }

        public void AutoFitWidthOn_excelSheet()
        {
            _excelSheet.UsedRange.Columns.AutoFit();
        }

        public bool WriteLine(Dictionary<int, string> values, bool bold = false, bool italic = false, bool underline = false)
        {
            foreach (int key in values.Keys)
            {
                OutputCell(CurrentRow, key, values[key]);
                _excelSheet.Cells[CurrentRow, key].EntireColumn.NumberFormat = "@";
                if (bold) MakeCellBold(CurrentRow, key);
                if (italic) MakeCellItalic(CurrentRow, key);
                if (underline) MakeCellUnderline(CurrentRow, key);
            }
            if (values.Count > 0)
                CurrentRow++;
            return true;
        }

        #endregion
    }
}
