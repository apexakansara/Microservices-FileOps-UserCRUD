using System.Text;
using FileOpsService.Helpers;
using FileOpsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileOpsService.Controllers;


[ApiController]
[Route("api/v{version:apiVersion}/file")]
public class FileController : ControllerBase
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ApiVersion("1.0")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, 
        IMessageProducer messageProducer,
        ILogger<FileController> logger)
    {
        if (Path.GetExtension(file.FileName)!=".csv")
        {
            return BadRequest();
        }

        FileHelper fileHelper = new FileHelper();
        await fileHelper.ProcessFileDataAsync(file, messageProducer, logger);

        return Ok();
    }

    [HttpGet]
    [ApiVersion("1.0")]
    public IActionResult Download([FromQuery] int rows)
    {
        FileHelper fileHelper = new FileHelper();
        string csvContent = fileHelper.GenerateData(rows);
        var csvBytes = Encoding.UTF8.GetBytes(csvContent);

        Response.Headers.Add("Content-Disposition", "attachment; filename=sample.csv");
        Response.ContentType = "text/csv";

        return File(csvBytes, "text/csv");
    }
}