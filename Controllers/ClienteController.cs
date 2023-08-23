using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaDapper.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cors;
using PruebaDapper.EXCELGenerator;
using PruebaDapper.PDFGenerator;

namespace PruebaDapper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly string connectionString;
        private readonly PDFCliente _pdf;
        private readonly EXCELCliente _excel;

        public ClienteController(ILogger<ClienteController> logger, IConfiguration configuration, PDFCliente pdf, EXCELCliente excel)
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

            string query = "SELECT * FROM Cliente";

            var clientes = await connection.QueryAsync(query);
            
            return Ok(clientes);
        }

        [EnableCors]
        [HttpGet("GetPDF")]
        public async Task<IActionResult> GetPDF()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM Cliente";
            List<Cliente> cliente;
            cliente = (await connection.QueryAsync<Cliente>(query)).ToList();

           var pdf =  await _pdf.GeneratePDF(cliente);


            return File(pdf, "application/pdf");
        }


        [EnableCors]
        [HttpGet("GetExcel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcel()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            //   string outputPath = "InformesTEMP/temporal.xlsx";

            // Tipo de informe para el encabezado
            string reportType = "Informe de Clientes";
            string query = "SELECT * FROM Cliente";

            List<Cliente> cliente;
            cliente = (await connection.QueryAsync<Cliente>(query)).ToList();


            // Generar el archivo PDF utilizando el método GeneratePDF
            var excel = await _excel.GenerateExcelClientes(cliente, reportType);

            
            return File(excel, "application/xlsx", "reporteclientes.xlsx");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody]Cliente cliente)
        {
            string sql = "sp_post_clientes";

            using (var connection = new SqlConnection(connectionString))
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new {
                    Ruc = cliente.Ruc.Trim(), 
                    RazonSocial = cliente.RazonSocial.Trim(),
                    Telefono = cliente.Telefono.Trim(),
                    Celular = cliente.Celular.Trim(),
                    Correo = cliente.Correo.Trim(),
                    Direccion = cliente.Direccion.Trim(),
                    Estado = cliente.Estado,
                }, commandType: CommandType.StoredProcedure);
                return Ok(rowsAffected);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody]Cliente cliente) 
        {
            string sql = "sp_update_clientes";

            using (var connection= new SqlConnection(connectionString))
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    Codigo = cliente.Codigo,
                    Ruc = cliente.Ruc.Trim(),
                    RazonSocial= cliente.RazonSocial.Trim(),
                    Telefono = cliente.Telefono.Trim(),
                    Celular = cliente.Celular.Trim(),
                    Correo = cliente.Correo.Trim(),
                    Direccion = cliente.Direccion.Trim(),
                    Estado = cliente.Estado,
                }, commandType: CommandType.StoredProcedure);

                return Ok(rowsAffected);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            string sql = "sp_delete_cliente";

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
