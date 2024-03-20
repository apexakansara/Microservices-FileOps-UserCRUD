using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;


public class CsvOutputFormatter : OutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        var dataList = context.Object as IEnumerable<object>;
        if(dataList!=null)
        {
            int count = 0;
            foreach (var data in dataList)
            {
                var properties = data.GetType().GetProperties();
                if(count==0)
                {
                    foreach(var property in properties)
                    {
                        buffer.Append($"{property.Name},");
                    }
                    buffer.AppendLine();
                    count++;
                }

                foreach (var property in properties)
                {
                    buffer.Append($"{property.GetValue(data)},");
                }
                buffer.AppendLine();
            }
        }

        await response.WriteAsync(buffer.ToString());
    }
}