namespace PruebaDapper.Models
{
    public class CabeceraVenta
    {
        public int? Numero { get; set; }
        public int? Cliente { get; set; }

        public DateTime FechaVenta { get; set; }
        public string? Observacion { get; set; }

        public List<DetalleVenta>? Detalle { get; set; }
    }
}
