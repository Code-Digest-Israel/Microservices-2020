using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Output_Service
{
    public class Startup
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private IServiceCollection _services;
        private static readonly HttpClient _http = new HttpClient();

        private const string dapr_url = "http://localhost:4003/v1.0/bindings/corona-notifications";

        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/dapr/subscribe", async context =>
                {
                    var data = new[] { new { Topic = "outbreak", Route = "outbreak" } };
                    var json = JsonSerializer.Serialize(data, _jsonOptions);

                    Console.WriteLine($"Subscribing for: '{json}'");

                    await context.Response.WriteAsync(json);
                });

                endpoints.MapPost("/outbreak", async context =>
                {
                    var message = await JsonSerializer.DeserializeAsync<Message>(context.Request.Body, _jsonOptions);

                    Console.WriteLine($"Outbreak in '{message.Data.City}' with '{message.Data.Patients}' patients");

                    await PublishOutbreak(message);
                
                    context.Response.StatusCode = 200;
                });
            });
        }

        private async Task PublishOutbreak(Message message)
        {            
            Console.WriteLine($"City: '{message.Data.City}', Patients: '{message.Data.Patients}'");

            var payload = new 
            {
                Data = new {
                    Target = "outbreak",
                    Arguments = new []
                    {
                        message.Data                                
                    }
                },
                Metadata = new {
                    Group = "outbreak-cities"
                },
                Operation = "create"
            };

            var json = JsonSerializer.Serialize(payload);

            var response = await _http.PostAsync(dapr_url, new StringContent(json));

            Console.WriteLine($"Publishing Success: {response.IsSuccessStatusCode}");
        }
    }

    public class Outbreak
    {
        public string City { get; set; }
        public int Patients { get; set; }
    }
    
    public class Message
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
        public string SpecVersion { get; set; }
        public string DataContentType { get; set; }
        public Outbreak Data { get; set; }
        public string Subject { get; set; }
    }

}