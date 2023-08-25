using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PruebaDapper.Models;
using System.Data;
using System.Data.SqlClient;
using PruebaDapper.EXCELGenerator;
using PruebaDapper.PDFGenerator;

namespace PruebaDapper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VentaController : Controller
    {
        private readonly ILogger<VentaController> _logger;
        private readonly string connectionString;
        private readonly  EXCELVentas _excel;
        private readonly PDFVenta _pdf;

        public VentaController(ILogger<VentaController> logger, IConfiguration configuration, EXCELVentas excel, PDFVenta pdf)
        {
            _logger = logger;
            connectionString = configuration.GetConnectionString("conexion");
            _excel = excel;
            _pdf = pdf;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            string query = "sp_obtener_cabecera";
            string detalle = "sp_obtener_detalle";


            using (var connection = new SqlConnection(connectionString))
            {
                var cabecera = connection.Query<CabeceraVenta>(query).ToList();
                foreach(var item in cabecera)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Numero", item.Numero);
                    var detalles = connection.Query<DetalleVenta>(detalle, parameters, commandType: CommandType.StoredProcedure).ToList();

                    item.Detalle = detalles;
                }
                return Ok(cabecera);
            }
        }

        [EnableCors]
        [HttpGet("GetExcel/{desde}/{hasta}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcel(DateTime desde, DateTime hasta)
        {
            string query = "sp_obtener_cabecera_reporte";
            string detalle = "sp_obtener_detalle";
            string reportType = "REPORTE DE VENTAS";


            var fechas = new DynamicParameters();
            fechas.Add("@Desde", desde);
            fechas.Add("@Hasta", hasta);


            using (var connection = new SqlConnection(connectionString))
            {
                var cabecera = connection.Query(query, fechas, commandType: CommandType.StoredProcedure).ToList();
                foreach (var item in cabecera)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Numero", item.Numero);
                    var detalles = connection.Query<DetalleVenta>(detalle, parameters, commandType: CommandType.StoredProcedure).ToList();

                    item.Detalle = detalles;
                }
                var excel = await _excel.GenerateExcelVentas(cabecera,reportType);
                return File(excel, "application/xlsx", "reporteVentas.xlsx");
            }
        }

        [EnableCors]
        [HttpGet("GetPDF/{desde}/{hasta}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPDF(DateTime desde, DateTime hasta)
        {
            string query = "sp_obtener_cabecera_reporte";
            string detalle = "sp_obtener_detalle";

            var fechas = new DynamicParameters();
            fechas.Add("@Desde", desde);
            fechas.Add("@Hasta", hasta);


            using (var connection = new SqlConnection(connectionString))
            {
                var cabecera = connection.Query(query, fechas, commandType: CommandType.StoredProcedure).ToList();
                foreach (var item in cabecera)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Numero", item.Numero);
                    var detalles = connection.Query<DetalleVenta>(detalle, parameters, commandType: CommandType.StoredProcedure).ToList();

                    item.Detalle = detalles;
                }

                var pdf = await _pdf.GeneratePDF(cabecera);

                return File(pdf, "application/pdf");
            }

            
        }

        [HttpGet("reporte/{desde}/{hasta}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReporte(DateTime desde, DateTime hasta)
        {
            string query = "sp_obtener_cabecera_reporte";
            string detalle = "sp_obtener_detalle";


            var fechas = new DynamicParameters();
            fechas.Add("@Desde", desde);
            fechas.Add("@Hasta", hasta);


            using (var connection = new SqlConnection(connectionString))
            {
                var cabecera = connection.Query(query, fechas, commandType: CommandType.StoredProcedure).ToList();
                foreach (var item in cabecera)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Numero", item.Numero);
                    var detalles = connection.Query<DetalleVenta>(detalle, parameters, commandType: CommandType.StoredProcedure).ToList();

                    item.Detalle = detalles;
                }
                return Ok(cabecera);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CabeceraVenta venta)
        {
            string sql = "sp_post_venta";
            string detalle = "sp_post_detalle";
            

            using (var connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Cliente", venta.Cliente);
                parameters.Add("@Fechaventa", venta.FechaVenta );
                parameters.Add("@Observacion", venta.Observacion);

                var result = connection.ExecuteScalar<int>("sp_post_venta", parameters, commandType: CommandType.StoredProcedure);

                foreach (var detalleVenta in venta.Detalle)
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@Numero", result);
                    parametros.Add("@Linea", detalleVenta.Linea);
                    parametros.Add("@Vehiculo", detalleVenta.Vehiculo);
                    parametros.Add("@Precio", detalleVenta.Precio);
                    parametros.Add("@Cantidad", detalleVenta.Cantidad);
                    parametros.Add("@Subtotal", detalleVenta.Subtotal);
                    parametros.Add("@Iva", detalleVenta.Iva);
                    parametros.Add("@Descuento", detalleVenta.Descuento);
                    parametros.Add("@Neto", detalleVenta.Neto);

                    var insertDetalle = connection.Query(detalle, parametros, commandType: CommandType.StoredProcedure);
                }

                return Ok(result);
            }
        }
    }
}
