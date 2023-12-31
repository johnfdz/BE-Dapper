﻿using iTextSharp.text.pdf;
using iTextSharp.text;
using PruebaDapper.Models;
using System.IO;

namespace PruebaDapper.PDFGenerator
{
    public class PDFVehiculo
    {
        public async Task<byte[]> GeneratePDF(List<Vehiculo> vehiculos)
        {
            var NombreEmpresa = "Birobid S.A";
            static Font FuenteTituloCabecera(float size) =>
                new(Font.FontFamily.HELVETICA, size, Font.BOLD, BaseColor.BLACK);
            Document doc = new Document();
            using var memoryStream = new MemoryStream();

            PdfWriter writer2 = PdfWriter.GetInstance(doc, memoryStream);

            using (FileStream fs = new FileStream("Archivo", FileMode.Create))
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
                cabecera.AddCell(new PdfPCell(new Phrase("INFORME DE VEHICULOS", FuenteTituloCabecera(9)))
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

                writer.Close();
                writer2.Close();
            }
            return memoryStream.ToArray();
        }
    }
}
