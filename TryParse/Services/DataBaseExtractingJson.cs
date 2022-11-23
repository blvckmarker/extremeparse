using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using TryParse.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace TryParse.Services
{
    public class DataBaseExtractingJson : IDataBaseExtracting
    {
        private IWebHostEnvironment webHost;


        public DataBaseExtractingJson(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }

        public async Task Export<TModel>(TModel entity, object? options) where TModel : IModel// туда
        {
            var cards = this.Import<TModel>(options.ToString());
            cards = cards.Append(entity);
            using (var JsonReader = File.OpenWrite(options.ToString()))
            {
                try
                {
                    await JsonSerializer.SerializeAsync(JsonReader, cards, new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public string DbPath
        {
            get
            {
                return Path.Combine(webHost.WebRootPath, "date", "cardDate.json");
            }
        }


        public async Task Export<TModel>(IEnumerable<TModel>? entity, object? options) where TModel : IModel// Сериализация
        {
            using (var JsonReader = File.OpenWrite((options as Settings).path))
            {
                try
                {
                    await JsonSerializer.SerializeAsync(JsonReader, entity, new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public IEnumerable<Model> Import<Model>(object? options) where Model : IModel // Десериализация
        {
            using (var JsonReader = File.OpenText(options.ToString()))
            {
                try
                {
                    return JsonSerializer.Deserialize<IEnumerable<Model>>(JsonReader.ReadToEnd(),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                }
                catch(JsonException)
                {
                    return Enumerable.Empty<Model>();
                }
            }
        }
        private record class Settings(string path);


    }

}
