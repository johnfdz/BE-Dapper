using iTextSharp.text.pdf;
using iTextSharp.text;
using PruebaDapper.Models;

namespace PruebaDapper.PDFGenerator
{
    public class PDFVenta
    {
        public async Task<byte[]> GeneratePDF(List<dynamic> ventas)
        {
            var NombreEmpresa = "Birobid S.A";
            static Font FuenteTituloCabecera(float size) =>
                new(Font.FontFamily.HELVETICA, size, Font.BOLD, BaseColor.BLACK);
            Document doc = new Document(PageSize.A4.Rotate());
            using var memoryStream = new MemoryStream();

            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fontHeader = new Font(baseFont, 12, Font.BOLD);

            PdfWriter writer2 = PdfWriter.GetInstance(doc, memoryStream);

            using (FileStream fs = new FileStream("Archivo.pdf", FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                var cabecera = new PdfPTable(new[] { 80f, 7f, 13f })
                {
                    WidthPercentage = 103f,
                };
                // 1 columna 1 fila
                cabecera.AddCell(new PdfPCell(new Phrase(NombreEmpresa, new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD, BaseColor.BLACK)))
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
                cabecera.AddCell(new PdfPCell(new Phrase("REPORTE DE VENTAS", FuenteTituloCabecera(9)))
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

                // Agregar tabla con lista de vehículos
                PdfPTable table = new PdfPTable(12);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1 });

                // Encabezados de columna
                table.AddCell(new PdfPCell(new Phrase("Numero", fontHeader)));
                table.AddCell(new PdfPCell(new Phrase("Cliente", fontHeader)));
                table.AddCell(new PdfPCell(new Phrase("Fecha de Venta", fontHeader)));
                table.AddCell(new PdfPCell(new Phrase("Observacion", fontHeader)));
                table.AddCell(new PdfPCell(new Phrase("Linea", fontHeader)));
                table.AddCell(new PdfPCell(new Phrase("Vehiculo", fontHeader)));
                table.AddCell(new PdfPCell(new Phrase("Precio", fontHeader)));
                table.AddCell(new PdfPCell(new Phrase("Cantidad", fontHeader)));
                table.AddCell(new PdfPCell(new Phrase("Subtotal", fontHeader)));
                table.AddCell(new PdfPCell(new Phrase("Iva", fontHeader)));
                table.AddCell(new PdfPCell(new Phrase("Descuento", fontHeader)));
                table.AddCell(new PdfPCell(new Phrase("Neto", fontHeader)));

                // Datos de los vehículos
                foreach (var venta in ventas)
                {
                    table.AddCell(venta.Numero.ToString());
                    table.AddCell(venta.Cliente);
                    table.AddCell(venta.FechaVenta.ToString("yyyy-MM-dd"));
                    table.AddCell(venta.Observacion);
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");

                    foreach (var detalle in venta.Detalle)
                    {
                        table.AddCell("");
                        table.AddCell("");
                        table.AddCell("");
                        table.AddCell("");
                        table.AddCell(detalle.Linea.ToString());
                        table.AddCell(detalle.Vehiculo.ToString());
                        table.AddCell(detalle.Precio.ToString());
                        table.AddCell(detalle.Cantidad.ToString());
                        table.AddCell(detalle.Subtotal.ToString());
                        table.AddCell(detalle.Iva.ToString());
                        table.AddCell(detalle.Descuento.ToString());
                        table.AddCell(detalle.Neto.ToString());
                    }
                }

                doc.Add(table);

                doc.Close();

                writer.Close();
                writer2.Close();
            }
            return memoryStream.ToArray();
        }
    }
}
