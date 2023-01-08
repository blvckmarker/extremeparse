using System.Text.Json;
using TryParse.Controllers;
using TryParse.Models;

namespace TryParse.Services.Extracting
{
    public class DataBaseExtractingJson : IDataBaseExtracting
    {
        private readonly ILogger logger;

        public async Task Export<TModel>(TModel entity, object? options) where TModel : IModel
        {
            logger.LogInformation($"[Json] Record a new entity | GUID : {entity.Id}");

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
                    logger.LogError($"[Json] ex.Message");
                }
            }
        }

        public DataBaseExtractingJson(ILogger<UserController> logger)
        {
            this.logger = logger;
            logger.LogInformation($"[Json] Create service - {this.GetHashCode()}");
        }

        public string DbPath
        {
            get => Path.Combine("wwwroot", "date", "cardDate.json");
        }


        public async Task Export<TModel>(IEnumerable<TModel>? entity, object? options) where TModel : IModel
        {
            logger.LogInformation($"[Json] Record a new entities | Count: {entity.Count()}");
            using (var JsonReader = File.OpenWrite(options.ToString()))
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
                    logger.LogError($"[Json] {ex.Message}");
                }
            }
        }

        public IEnumerable<TModel> Import<TModel>(object? options) where TModel : IModel
        {
            using var JsonReader = File.OpenText(options.ToString());
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

        public async Task Remove<TModel>(string GUID) where TModel : IModel
        {
            throw new NotImplementedException();
        }

    }
}
