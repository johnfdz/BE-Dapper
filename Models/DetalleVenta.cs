namespace PruebaDapper.Models
{
    public class DetalleVenta
    {
        public int? Numero { get; set; }
        public int? Linea { get; set; }
        public int? Vehiculo { get; set; }
        public float? Precio { get; set; }
        public int? Cantidad { get; set; }
        public float? Subtotal { get; set; }
        public float? Iva { get; set; }
        public float? Descuento { get; set; }
        public float? Neto { get; set; }
    }
}
