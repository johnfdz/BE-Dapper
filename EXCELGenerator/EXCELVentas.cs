using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using Color = System.Drawing.Color;

namespace PruebaDapper.EXCELGenerator
{
    public class EXCELVentas
    {
        public async Task<Stream> GenerateExcelVentas(List<dynamic> ventas, string reportType)
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



                sl.SetCellValue(5, 1, "Numero");
                sl.SetCellValue(5, 2, "Cliente");
                sl.SetCellValue(5, 3, "Fecha");
                sl.SetCellValue(5, 4, "Observacion");
                sl.SetCellValue(5, 5, "Linea");
                sl.SetCellValue(5, 6, "Vehiculo");
                sl.SetCellValue(5, 7, "Precio");
                sl.SetCellValue(5, 8, "Cantidad");
                sl.SetCellValue(5, 9, "Subtotal");
                sl.SetCellValue(5, 10, "Iva");
                sl.SetCellValue(5,11, "Descuento");
                sl.SetCellValue(5, 12, "Neto");

                sl.SetCellStyle(5, 1, 5, 12, tableHeader);

                int rowIndex = 6;

                foreach (var venta in ventas)
                {   
                    

                    sl.SetCellValue(rowIndex, 1, venta.Numero.ToString());
                    sl.SetCellValue(rowIndex, 2, venta.Cliente);
                    sl.SetCellValue(rowIndex, 3, venta.FechaVenta.ToString("yyyy-MM-dd"));
                    sl.SetCellValue(rowIndex, 4, venta.Observacion);

                    foreach (var detalle in venta.Detalle)
                    {
                        sl.SetCellValue(rowIndex, 5, detalle.Linea);
                        sl.SetCellValue(rowIndex, 6, detalle.Vehiculo);
                        sl.SetCellValue(rowIndex, 7, detalle.Precio.ToString());
                        sl.SetCellValue(rowIndex, 8, detalle.Cantidad.ToString());
                        sl.SetCellValue(rowIndex, 9, detalle.Subtotal.ToString());
                        sl.SetCellValue(rowIndex, 10, detalle.Iva.ToString());
                        sl.SetCellValue(rowIndex, 11, detalle.Descuento.ToString());
                        sl.SetCellValue(rowIndex, 12, detalle.Neto.ToString());
                        rowIndex++;
                    }
                    sl.MergeWorksheetCells(rowIndex - venta.Detalle.Count, 1, rowIndex - 1, 1);
                    rowIndex++;
                }
                sl.AutoFitColumn(1, 20);
                sl.AutoFitRow(1, rowIndex + 5);

                sl.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }
    }
}
