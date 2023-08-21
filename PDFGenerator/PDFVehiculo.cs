using iTextSharp.text.pdf;
using iTextSharp.text;
using PruebaDapper.Models;

namespace PruebaDapper.PDFGenerator
{
    public class PDFVehiculo
    {
        public void GeneratePDF(List<Vehiculo> vehiculos, string outputPath, string reportType)
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

                // Agregar tabla con lista de vehículos
                PdfPTable table = new PdfPTable(5); // Número de columnas
                table.WidthPercentage = 100; // Ancho de la tabla en porcentaje

                // Encabezados de columna
                table.AddCell("Código");
                table.AddCell("Nombre");
                table.AddCell("Marca");
                table.AddCell("Precio");
                table.AddCell("Estado");

                // Datos de los vehículos
                foreach (var vehiculo in vehiculos)
                {
                    table.AddCell(vehiculo.Codigo.ToString());
                    table.AddCell(vehiculo.Nombre);
                    table.AddCell(vehiculo.Marca);
                    table.AddCell(vehiculo.Precio.ToString());
                    table.AddCell((bool)vehiculo.Estado ? "Activo" : "Inactivo");
                }

                doc.Add(table);

                doc.Close();
            }
        }
    }
}
