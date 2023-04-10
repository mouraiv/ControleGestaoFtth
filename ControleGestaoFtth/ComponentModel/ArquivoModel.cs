using OfficeOpenXml;

namespace ControleGestaoFtth.ComponentModel
{
    public class ArquivoModel
    {
        public string? Nome { get; set; }
        public string ?Caminho { get; set; }
        public int TamanhoTotalXlsx { get; set; }

        public void ImportarXlsx(Stream file)
        {
            if (file != null)
            {
                using (var pacote = new ExcelPackage(file))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    var planilha = pacote.Workbook.Worksheets[0];
                    var totalRows = planilha.Dimension.End.Row;
                    var totalColumns = planilha.Dimension.End.Column;
                    var totalCells = totalRows * totalColumns;

                    int nonEmptyCells = 0;
                    for (int row = 1; row <= totalRows; row++)
                    {
                        for (int col = 1; col <= totalColumns; col++)
                        {
                            var cellValue = planilha.Cells[row, col].Value;
                            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                            {
                                TamanhoTotalXlsx = nonEmptyCells++;
                            }
                        }
                    }
                }
            }
        }
    }
}
