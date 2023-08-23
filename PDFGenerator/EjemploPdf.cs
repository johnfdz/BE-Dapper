using iTextSharp.text.pdf;
using iTextSharp.text;
using PruebaDapper.Models;

namespace PruebaDapper.PDFGenerator
{
    public class EjemploPdf
    {
        public async Task<byte[]> GeneratePDF(List<Cliente> clientes)
        {
      
            static Font FuenteTituloCabecera(float size) =>
            new(Font.FontFamily.HELVETICA, size, Font.BOLD, BaseColor.BLACK);
            using var memoryStream = new MemoryStream();
            Document doc = new Document();

            using (FileStream fs = new FileStream("Archivo", FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                PdfWriter writer2 = PdfWriter.GetInstance(doc, memoryStream);

                doc.Open();

                var cabecera = new PdfPTable(new[] { 80f, 7f, 13f })
                {
                    WidthPercentage = 103f,
                };
                // 1 columna 1 fila
                cabecera.AddCell(new PdfPCell(new Phrase("BIROBID", new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD, BaseColor.BLACK)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,

                });
                // 2 columna 1 fila
                cabecera.AddCell(new PdfPCell(new Phrase("Fecha:", new Font(Font.FontFamily.HELVETICA, 10)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                });

                // 3 columna 1 fila
                cabecera.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 10)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                });

                // 1 columna 2 fila
                cabecera.AddCell(new PdfPCell(new Phrase("INFORME DE CLIENTES", FuenteTituloCabecera(9)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,


                });
                cabecera.AddCell(new PdfPCell(new Phrase("Hora:", new Font(Font.FontFamily.HELVETICA, 10)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                });

                cabecera.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToShortTimeString(), new Font(Font.FontFamily.HELVETICA, 10)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                });
                // Agregar encabezado
                doc.Add(cabecera);

                doc.Add(new Paragraph(" "));

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

                //cabecera.AddCell(new PdfPCell(table)
                //{
                //    Border = Rectangle.NO_BORDER,
                //    Colspan = 8,
                //});

                doc.Add(table);
                doc.Close();    
                writer.Close();
                writer2.Close();   
            }
            return memoryStream.ToArray();
        }
    }
}
