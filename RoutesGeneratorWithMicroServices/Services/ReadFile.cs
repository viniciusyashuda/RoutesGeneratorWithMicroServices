using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace RoutesGeneratorWithMicroServices.Services
{
    public class ReadFile
    {
        public static List<string> ReadHeader(string pathWebRoot)
        {
            List<string> header = new();

            FileInfo existingFile = new FileInfo(pathWebRoot + "\\file\\Plan220258188.xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[PositionID: 0];
                int columnCount = worksheet.Dimension.End.Column;

                for (int column = 1; column < columnCount; column++)
                    header.Add(worksheet?.Cells[Row: 1, column]?.Value?.ToString());
            }
            return header;
        }

        public static List<IDictionary<string, string>> ReadExcelFile(List<string> columns, string pathWebRoot)
        {
            List<string> plan = new();
            List<IDictionary<string, string>> listDictionary = new();

            FileInfo excelFile = new FileInfo(pathWebRoot + "\\file\\Plan220258188.xlsx");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new(excelFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int total_columns = worksheet.Dimension.End.Column;
                int total_rows = worksheet.Dimension.End.Row;

                IDictionary<string, string> data = new Dictionary<string, string>();

                for (int row = 2; row < total_rows; row++)
                {
                    data = new Dictionary<string, string>();

                    for (int col = 1; col < total_columns; col++)
                    {
                        columns.ForEach(column =>
                        {
                            if (worksheet.Cells[1, col].Value.ToString() == column)
                            {
                                if (worksheet.Cells[row, col].Value == null)
                                    data.Add(column, "");
                                else
                                    data.Add(column, worksheet.Cells[row, col].Value.ToString());
                            }
                        });

                    }
                    if (data.Count > 1)
                        listDictionary.Add(data);
                }
            }

            return listDictionary;
        }

        public static List<string> ReadColumn(string columnName, string pathWebRoot)
        {
            List<string> columnContent = new();

            FileInfo excelFile = new FileInfo(pathWebRoot + "\\file\\Plan220258188.xlsx");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new(excelFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int total_columns = worksheet.Dimension.End.Column;
                int total_rows = worksheet.Dimension.End.Row;

                for (int column = 1; column < total_columns; column++)
                {
                    if (worksheet.Cells[1, column].Value.ToString() == columnName)
                        for (int row = 2; row < total_rows; row++)
                        {
                            if (worksheet.Cells[row, column].Value == null)
                                columnContent.Add("");
                            else
                                columnContent.Add(worksheet.Cells[row, column].Value.ToString());
                        }
                }
            }

            return columnContent;
        }
    }
}
