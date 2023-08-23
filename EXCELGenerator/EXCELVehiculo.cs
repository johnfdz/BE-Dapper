using DocumentFormat.OpenXml.Spreadsheet;
using PruebaDapper.Models;
using SpreadsheetLight;
using Color = System.Drawing.Color;

namespace PruebaDapper.EXCELGenerator
{
    public class EXCELVehiculo
    {
        public async Task<Stream> GenerateExcelVehiculos(List<Vehiculo> vehiculos, string reportType)
        {

            SLStyle sheetStyle = new SLStyle();
            sheetStyle.SetFontBold(true);
            //sheetStyle.SetPatternFill(PatternValues.,Color.Empty, Color.Coral);
            sheetStyle.Fill.SetPattern(PatternValues.Solid, Color.LightSkyBlue, Color.Empty);
            sheetStyle.SetFont(FontSchemeValues.Major, 20);

            SLStyle tableHeader = new SLStyle();
            tableHeader.SetFontBold(true);
            tableHeader.Fill.SetPattern(PatternValues.Solid, Color.AliceBlue, Color.Empty);

            MemoryStream ms = new();

            using (var sl = new SLDocument())
                
            {
                // Encabezados
                sl.SetCellValue(3, 1, "Fecha");
                sl.SetCellValue(3, 3, "Hora");

                sl.SetCellValue(1, 1, reportType);
                sl.SetCellStyle(1, 1, sheetStyle);
                sl.SetCellValue(3, 2, DateTime.Now.ToShortDateString());
                sl.SetCellValue(3, 4, DateTime.Now.ToShortTimeString());

                sl.MergeWorksheetCells(1, 1, 2, 4);
                
                

                sl.SetCellValue(5, 1, "Código");
                sl.SetCellValue(5, 2, "Nombre");
                sl.SetCellValue(5, 3, "Marca");
                sl.SetCellValue(5, 4, "Precio");
                sl.SetCellValue(5, 5, "Estado");

                sl.SetCellStyle(5, 1, 5, 5, tableHeader);

                int rowIndex = 6;

                foreach (var vehiculo in vehiculos)
                {
                    sl.SetCellValue(rowIndex, 1, vehiculo.Codigo.ToString());
                    sl.SetCellValue(rowIndex, 2, vehiculo.Nombre);
                    sl.SetCellValue(rowIndex, 3, vehiculo.Marca);
                    sl.SetCellValue(rowIndex, 4, vehiculo.Precio.ToString());
                    sl.SetCellValue(rowIndex, 5, (bool)vehiculo.Estado ? "Activo" : "Inactivo");
                    rowIndex++;
                }
                sl.AutoFitColumn(1, 20);
                sl.AutoFitRow(1, rowIndex +5);

                sl.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }
    }
}
