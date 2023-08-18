using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using PruebaDapper.Models;
using System.Data;

namespace PruebaDapper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehiculoController : Controller
    {

        private readonly ILogger<VehiculoController> _logger;
        private readonly string connectionString;

        public VehiculoController(ILogger<VehiculoController> logger, IConfiguration configuration)
        {
            _logger = logger;
            connectionString = configuration.GetConnectionString("conexion");

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
