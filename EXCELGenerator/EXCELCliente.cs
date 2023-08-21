using PruebaDapper.Models;
using SpreadsheetLight;

namespace PruebaDapper.EXCELGenerator
{
    public class EXCELCliente
    {
        public void GenerateExcelClientes(List<Cliente> clientes, string outputPath, string reportType)
        {
            using (var sl = new SLDocument())
            {
                // Encabezados
                sl.SetCellValue(1, 1, "Título");
                sl.SetCellValue(1, 2, "Fecha");
                sl.SetCellValue(1, 3, "Hora");
                sl.SetCellValue(1, 4, "Tipo de Documento");

                sl.SetCellValue(2, 1, reportType);
                sl.SetCellValue(2, 2, DateTime.Now.ToShortDateString());
                sl.SetCellValue(2, 3, DateTime.Now.ToShortTimeString());

                //sl.MergeWorksheetCells(2, 1, 2, 3);
                sl.AutoFitColumn(1, 4);

                sl.SetCellValue(4, 1, "Código");
                sl.SetCellValue(4, 2, "RUC");
                sl.SetCellValue(4, 3, "Razón Social");
                sl.SetCellValue(4, 4, "Teléfono");
                sl.SetCellValue(4, 5, "Celular");
                sl.SetCellValue(4, 6, "Correo");
                sl.SetCellValue(4, 7, "Dirección");
                sl.SetCellValue(4, 8, "Estado");

                int rowIndex = 5;
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

                sl.SaveAs(outputPath);
            }
        }
    }
}
