using System.Text.Json;
using TryParse.Models;

namespace TryParse.Services.Extracting
{
    public class DataBaseExtractingJson : IDataBaseExtracting
    {
        public async Task Export<TModel>(TModel entity, object? options) where TModel : IModel
        {
            var cards = Import<TModel>(options.ToString()); // TODO: optimize
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
                return Path.Combine("wwwroot", "date", "cardDate.json");
            }
        }


        public async Task Export<TModel>(IEnumerable<TModel>? entity, object? options) where TModel : IModel
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

        public IEnumerable<TModel> Import<TModel>(object? options) where TModel : IModel
        {
            using (var JsonReader = File.OpenText(options.ToString()))
            {
                try
                {
                    return JsonSerializer.Deserialize<IEnumerable<TModel>>(JsonReader.ReadToEnd(),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                }
                catch (JsonException)
                {
                    return Enumerable.Empty<TModel>();
                }
            }
        }
        private record class Settings(string path);


    }

}
