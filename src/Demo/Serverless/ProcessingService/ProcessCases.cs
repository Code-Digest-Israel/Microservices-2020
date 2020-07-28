using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json.Linq;

namespace ProcessingService
{
  public static class ProcessCases
  {
    [FunctionName("process-cases-updates")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        [SignalR(HubName = "outbreaks")] IAsyncCollector<SignalRMessage> signalRMessages,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      string bodyString = await req.ReadAsStringAsync();
      JObject body = JsonConvert.DeserializeObject<JObject>(bodyString);
      int added = int.Parse(body["added"].ToString());
      if (added >= 100)
      {
        string city = body["city"].ToString();
        await signalRMessages.AddAsync(
          new SignalRMessage
          {
            Target = "outbreak",
            Arguments = new[] { new { City = city, Patients = added } }
          }
        );

        await signalRMessages.FlushAsync();
      }

      return new OkResult();
    }
  }
}
