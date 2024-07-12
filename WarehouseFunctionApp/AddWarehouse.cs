using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WarehouseFunctionApp.Data;
using WarehouseFunctionApp.Models;

namespace WarehouseFunctionApp
{
    public class AddWarehouse
    {
        private readonly WarehouseContext _context;

        public AddWarehouse(WarehouseContext context)
        {
            _context = context;
        }

        [Function("AddWarehouse")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("AddWarehouse");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Warehouse>(requestBody);


            if (data == null || string.IsNullOrEmpty(data.Name) || data.FarmId <= 0)
            {
                return new BadRequestObjectResult("Invalid input");
            }

            // data.Id = 1;

            _context.Warehouses.Add(data);
            await _context.SaveChangesAsync();

            return new OkObjectResult(data);
        }
    }
}
