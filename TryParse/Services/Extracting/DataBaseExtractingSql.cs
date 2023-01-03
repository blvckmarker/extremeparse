using TryParse.Models;
using TryParse.Models.Context;

namespace TryParse.Services.Extracting
{
    public class DataBaseExtractingSql : IDataBaseExtracting
    {
        private readonly ModelContext db = new();
        public async Task Export<TModel>(TModel entity, object? options = null) where TModel : IModel
        {
            await db.Models.AddAsync(entity as CardModel);
            db.SaveChanges();
        }

        public async Task Export<TModel>(IEnumerable<TModel>? entity, object? options = null) where TModel : IModel
        {
            try
            {
                foreach (var item in entity)
                    await db.Models.AddAsync(item as CardModel);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            db.SaveChanges();
        }

        public IEnumerable<TModel> Import<TModel>(object? options) where TModel : IModel => db.Models as IEnumerable<TModel>;


        public string DbPath
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

