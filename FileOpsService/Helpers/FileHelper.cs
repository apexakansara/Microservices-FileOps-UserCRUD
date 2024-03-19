using System;
using System.Globalization;
using System.Text;
using ConsumerService.Models;
using CsvHelper;
using CsvHelper.Configuration;
using FileOpsService.Models;
using Newtonsoft.Json;

namespace FileOpsService.Helpers;

public class FileHelper
{
    public async Task ProcessFileDataAsync(IFormFile file, 
        IMessageProducer messageProducer,
        ILogger logger)
    {
        List<string> content = await ReadContentAsync(file, logger);
        messageProducer.Publish(content);
    }

    public string GenerateData(int count)
    {
        StringBuilder csvContent = new StringBuilder ();

        List<string> companies = new List<string>() { 
            "TCS", "Google", "Meta", "Netflix", "Infosys", "Amazon"};
        List<string> domains = new List<string>() { 
            "example.com", "test.com", "fakeemail.com" };
        Random random = new Random();

        
        csvContent.AppendLine("Name,PhoneNumber,Email,CompanyName");

        for(int i=0; i<count; i++)
        {
            csvContent.AppendLine(@$"User{i},
                9999988888,
                user{i}@{domains[random.Next(domains.Count)]},
                {companies[random.Next(companies.Count)]}");
        }

        return csvContent.ToString();
    }

    private async Task<List<string>> ReadContentAsync(IFormFile file, ILogger logger)
    {
        List<string> content = new List<string>();
        Employee employee;
        CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true
        };

        try
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                while (await csv.ReadAsync())
                {
                    employee = csv.GetRecord<Employee>();
                    Message msg = new Message() { 
                        Type = "AddEmployee", 
                        Body = JsonConvert.SerializeObject(employee)};
                    content.Add(JsonConvert.SerializeObject(msg));

                }
            }
        }
        catch(Exception ex)
        {
            logger.LogError($"Error {ex.Message} at {ex.StackTrace}");
        }

        return content;
    }
} 