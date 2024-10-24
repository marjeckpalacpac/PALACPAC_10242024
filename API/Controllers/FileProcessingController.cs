using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileProcessingController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Upload([FromForm] IFormFile? file)
        {

            if (file == null || file.Length == 0)
            {
                return BadRequest(new ProblemDetails { Title = "Invalid file" });
            }

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (extension == ".csv")
            {
                var result = await ProcessCsvFile(file);
                return result;
            }
            else if (extension == ".json")
            {
                var result = await ProcessJsonFile(file);
                return result;
            }
            else
            {
                return BadRequest(new ProblemDetails { Title = "Unsupported file format" });
            }

        }

        private async Task<ActionResult> ProcessCsvFile(IFormFile file)
        {
            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var csvLines = new List<string>();
                string? line;

                while ((line = await stream.ReadLineAsync()) != null)
                {
                    csvLines.Add(line);
                }

                if (!csvLines.Any())
                {
                    return BadRequest(new ProblemDetails { Title = "Empty or invalid CSV file" });
                }

                var headerLine = csvLines[0];
                var headers = headerLine.Split(',');

                var expectedColumns = new List<string> { "Product", "QuantitySold", "PricePerUnit", "TotalPrice" };
                var missingColumns = expectedColumns.Except(headers).ToList();
                if (missingColumns.Any())
                {
                    return BadRequest(new ProblemDetails { Title = $"Missing columns: {string.Join(", ", missingColumns)}" });
                }

                var totalQuantity = 0.0;
                var totalPriceSum = 0.0;
                var totalCount = 0;

                foreach (var csvRow in csvLines.Skip(1))
                {
                    var values = csvRow.Split(',');

                    if (double.TryParse(values[Array.IndexOf(headers, "QuantitySold")], out double quantity))
                    {
                        totalQuantity += quantity;
                    }

                    if (double.TryParse(values[Array.IndexOf(headers, "PricePerUnit")], out double pricePerUnit))
                    {
                        totalPriceSum += pricePerUnit;
                    }

                    totalCount++;
                }

                if (totalCount == 0)
                {
                    return BadRequest(new ProblemDetails { Title = "No valid data to calculate averages." });
                }

                var averageQuantity = (double)totalQuantity / totalCount;
                var averagePricePerUnit = totalPriceSum / totalCount;
                return Ok(new
                {
                    Message = "CSV file processed successfully",
                    AverageQuantitySold = averageQuantity,
                    AveragePricePerUnit = averagePricePerUnit
                });
            }
        }

        private async Task<ActionResult> ProcessJsonFile(IFormFile file)
        {
            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var jsonString = await stream.ReadToEndAsync();

                var products = JsonSerializer.Deserialize<List<Product>>(jsonString);

                if (products == null || !products.Any())
                {
                    return BadRequest(new ProblemDetails { Title = "Invalid or empty JSON file" });
                }

                var filteredProducts = products.Where(p => p.PricePerUnit > 100).ToList();

                return Ok(new
                {
                    Message = "Json file processed successfully",
                    FilteredProducts = filteredProducts,
                });

            }
        }
    }
}
