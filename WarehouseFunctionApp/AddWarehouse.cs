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
            ILogger logger = executionContext.GetLogger("AddWarehouse");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody;
            try
            {
                requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                logger.LogInformation("Request body read successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error reading request body: {ex.Message}");
                return new BadRequestObjectResult("Failed to read request body");
            }

            Warehouse data;
            try
            {
                data = JsonConvert.DeserializeObject<Warehouse>(requestBody);
                logger.LogInformation("Request body deserialized successfully.");
            }
            catch (JsonException ex)
            {
                logger.LogError($"Error deserializing request body: {ex.Message}");
                return new BadRequestObjectResult("Invalid request body");
            }

            if (data == null || string.IsNullOrEmpty(data.Name) || data.FarmId <= 0)
            {
                logger.LogWarning("Invalid input data.");
                return new BadRequestObjectResult("Invalid input");
            }

            logger.LogInformation($"Warehouse Data: Name = {data.Name}, FarmId = {data.FarmId}");

            // data.Id = 1;

            try
            {
                _context.Warehouses.Add(data);
                await _context.SaveChangesAsync();
                logger.LogInformation("Warehouse data saved successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error saving warehouse data: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult(data);
        }
    }
}
