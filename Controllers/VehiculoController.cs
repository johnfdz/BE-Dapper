using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using PruebaDapper.Models;
using System.Data;
using Microsoft.AspNetCore.Cors;
using PruebaDapper.EXCELGenerator;
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
        private readonly EXCELVehiculo _excel;

        public VehiculoController(ILogger<VehiculoController> logger, IConfiguration configuration, PDFVehiculo pdf, EXCELVehiculo excel)
        {
            _logger = logger;
            connectionString = configuration.GetConnectionString("conexion");
            _pdf = pdf;
            _excel = excel;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {

            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT Codigo, Nombre, Marca, Precio, Estado FROM Vehiculo";

            var vehiculos = await connection.QueryAsync(query);

            return Ok(vehiculos);
        }

        [EnableCors]
        [HttpGet("GetPDF")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPDF()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT Codigo, Nombre, Marca, Precio, Estado FROM Vehiculo";

            List<Vehiculo> vehiculos = new List<Vehiculo>();

            vehiculos = (await connection.QueryAsync<Vehiculo>(query)).ToList();

            var pdf = await _pdf.GeneratePDF(vehiculos);

            return File(pdf,"application/pdf");
        }


        [EnableCors]
        [HttpGet("GetExcel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcel()
        {
            // Tipo de informe para el encabezado
            string reportType = "Informe de Vehiculos";

            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT Codigo, Nombre, Marca, Precio, Estado FROM Vehiculo";

            List<Vehiculo> vehiculos = new List<Vehiculo>();

            vehiculos = (await connection.QueryAsync<Vehiculo>(query)).ToList();

            var excel = await _excel.GenerateExcelVehiculos(vehiculos, reportType);

            return File(excel, "application/xlsx", "reportevehiculos.xlsx");
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
