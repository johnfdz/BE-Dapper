﻿using DocumentFormat.OpenXml.Spreadsheet;
using PruebaDapper.Models;
using SpreadsheetLight;
using Color = System.Drawing.Color;

namespace PruebaDapper.EXCELGenerator
{
    public class EXCELCliente
    {
        public async Task<Stream> GenerateExcelClientes(List<Cliente> clientes, string reportType)
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

                sl.AutoFitColumn(1, 4);

                sl.SetCellValue(5, 1, "Código");
                sl.SetCellValue(5, 2, "RUC");
                sl.SetCellValue(5, 3, "Razón Social");
                sl.SetCellValue(5, 4, "Teléfono");
                sl.SetCellValue(5, 5, "Celular");
                sl.SetCellValue(5, 6, "Correo");
                sl.SetCellValue(5, 7, "Dirección");
                sl.SetCellValue(5, 8, "Estado");

                sl.SetCellStyle(5, 1, 5, 8, tableHeader);

                int rowIndex = 6;
                foreach (var cliente in clientes)
                {
                    sl.SetCellValue(rowIndex, 1, cliente.Codigo.ToString());
                    sl.SetCellValue(rowIndex, 2, cliente.Ruc);
                    sl.SetCellValue(rowIndex, 3, cliente.RazonSocial);
                    sl.SetCellValue(rowIndex, 4, cliente.Telefono);
                    sl.SetCellValue(rowIndex, 5, cliente.Celular);
                    sl.SetCellValue(rowIndex, 6, cliente.Correo);
                    sl.SetCellValue(rowIndex, 7, cliente.Direccion);
                    sl.SetCellValue(rowIndex, 8,(bool) cliente.Estado ? "Activo" : "Inactivo");
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
