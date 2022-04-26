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

            FileInfo existingFile = new FileInfo(pathWebRoot + "\\file\\Plan.xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[PositionID: 0];
                int columnCount = worksheet.Dimension.End.Column;

                for (int column = 1; column < columnCount + 1; column++)
                    header.Add(worksheet?.Cells[Row: 1, column]?.Value?.ToString());
            }
            return header;
        }

        public static List<IDictionary<string, string>> ReadExcelFile(List<string> columns, string pathWebRoot)
        {
            List<string> plan = new();
            List<IDictionary<string, string>> listDictionary = new();

            FileInfo excelFile = new FileInfo(pathWebRoot + "\\file\\Plan.xlsx");

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

                    for (int col = 1; col < total_columns + 1; col++)
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

            FileInfo excelFile = new FileInfo(pathWebRoot + "\\file\\Plan.xlsx");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new(excelFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int total_columns = worksheet.Dimension.End.Column;
                int total_rows = worksheet.Dimension.End.Row;

                for (int column = 1; column < total_columns + 1; column++)
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

        public static void OrderFile(string path)
        {
            List<string> headerExcel = new();
            FileInfo excelFile = new FileInfo(path);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new(excelFile))
            {
                ExcelWorkbook wb = package.Workbook;
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int cols = worksheet.Dimension.End.Column;
                int rows = worksheet.Dimension.End.Row;
                int colCep = 0;
                for (int col = 1; col <= cols; col++)
                {
                    if (worksheet.Cells[1, col].Value.ToString().ToUpper() == "CEP")
                    {
                        colCep = col - 1;
                        break;
                    }
                }
                worksheet.Cells[2, 1, rows, cols].Sort(colCep, false);
                package.Save();
            }
        }
    }
}
