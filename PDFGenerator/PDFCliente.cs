using iTextSharp.text;
using iTextSharp.text.pdf;
using PruebaDapper.Models;
using System.Collections.Generic;
using System.IO;


namespace PruebaDapper.PDFGenerator
{
    public class PDFCliente
    {
        public void GeneratePDF(List<Cliente> clientes, string outputPath, string reportType)
        {
            Document doc = new Document();

            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Agregar encabezado
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.EMBEDDED, 12);
                doc.Add(new Paragraph("Nombre del Informe: " + reportType, headerFont));
                doc.Add(new Paragraph("Fecha: " + DateTime.Now.ToShortDateString(), headerFont));
                doc.Add(new Paragraph("Hora: " + DateTime.Now.ToShortTimeString(), headerFont));
                doc.Add(new Paragraph("------------------------------------"));

                // Agregar tabla con lista de clientes
                PdfPTable table = new PdfPTable(8); // Número de columnas
                table.WidthPercentage = 100; // Ancho de la tabla en porcentaje

                // Encabezados de columna
                table.AddCell("Código");
                table.AddCell("RUC");
                table.AddCell("Razón Social");
                table.AddCell("Teléfono");
                table.AddCell("Celular");
                table.AddCell("Correo");
                table.AddCell("Dirección");
                table.AddCell("Estado");

                // Datos de los clientes
                foreach (var cliente in clientes)
                {
                    table.AddCell(cliente.Codigo.ToString());
                    table.AddCell(cliente.Ruc);
                    table.AddCell(cliente.RazonSocial);
                    table.AddCell(cliente.Telefono);
                    table.AddCell(cliente.Celular);
                    table.AddCell(cliente.Correo);
                    table.AddCell(cliente.Direccion);
                    table.AddCell((bool)cliente.Estado ? "Activo" : "Inactivo");
                }

                doc.Add(table);

                doc.Close();
            }
        }

    }
}
