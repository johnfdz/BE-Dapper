using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using PruebaDapper.Models;
using System.Data;
using PruebaDapper.PDFGenerator;

namespace PruebaDapper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehiculoController : Controller
    {

        private readonly ILogger<VehiculoController> _logger;
        private readonly string connectionString;
        private readonly PDFVehiculo _pdf;

        public VehiculoController(ILogger<VehiculoController> logger, IConfiguration configuration, PDFVehiculo pdf)
        {
            _logger = logger;
            connectionString = configuration.GetConnectionString("conexion");
            _pdf = pdf;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {

            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT Codigo, Nombre, Marca, Precio, Estado FROM Vehiculo";

            var clientes = await connection.QueryAsync(query);

            return Ok(clientes);
        }

        [HttpPost("GetPDF")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPDF([FromBody] List<Vehiculo> vehiculos)
        {
            string outputPath = "InformesTEMP/temporal.pdf";

            // Tipo de informe para el encabezado
            string reportType = "Informe de Vehiculos";

            // Generar el archivo PDF utilizando el método GeneratePDF
            _pdf.GeneratePDF(vehiculos, outputPath, reportType);

            // Devolver el archivo PDF como respuesta
            byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);
            System.IO.File.Delete(outputPath); // Eliminar el archivo temporal

            return File(fileBytes, "application/pdf", "repVehiculos.pdf");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] Vehiculo vehiculo)
        {
            string sql = "sp_post_vehiculo";

            using (var connection = new SqlConnection(connectionString))
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    Nombre = vehiculo.Nombre.Trim(),
                    Marca = vehiculo.Marca.Trim(),
                    Precio = vehiculo.Precio,
                    Estado = vehiculo.Estado,
                }, commandType: CommandType.StoredProcedure);
                return Ok(rowsAffected);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] Vehiculo vehiculo)
        {
            string sql = "sp_update_vehiculo";

            using (var connection = new SqlConnection(connectionString))
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    Codigo = vehiculo.Codigo,
                    Nombre = vehiculo.Nombre.Trim(),
                    Marca = vehiculo.Marca.Trim(),
                    Precio = vehiculo.Precio,
                    Estado = vehiculo.Estado,
                }, commandType: CommandType.StoredProcedure);

                return Ok(rowsAffected);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            string sql = "sp_delete_vehiculo";

            using (var connection = new SqlConnection(connectionString))
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    Codigo = id,
                }, commandType: CommandType.StoredProcedure);

                return Ok(rowsAffected);
            }
        }
    }
}
