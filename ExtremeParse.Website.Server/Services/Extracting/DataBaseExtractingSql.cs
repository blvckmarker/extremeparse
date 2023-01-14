using ExtremeParse.Controllers;
using ExtremeParse.Models;
using ExtremeParse.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace ExtremeParse.Services.Extracting
{
    public class DataBaseExtractingSql : IDataBaseExtracting
    {
        private readonly ModelContext db = new();
        private readonly ILogger logger;

        public async Task Export<TModel>(TModel entity, object? options = null) where TModel : IModel
        {
            logger.LogInformation($"[Sql] Record a new entity | GUID : {entity.Id}");
            try
            {
                await db.Models.AddAsync(entity as CardModel);
            }
            catch (OperationCanceledException exc)
            {
                logger?.LogWarning(exc.Message);
            }
            db.SaveChanges();
        }

        public DataBaseExtractingSql(ILogger<UserController> logger)
        {
            this.logger = logger;
            logger.LogInformation($"[Sql] Create service - {this.GetHashCode()}");
        }


        public async Task Export<TModel>(IEnumerable<TModel>? entity, object? options = null) where TModel : IModel
        {
            try
            {
                foreach (var item in entity)
                    await db.Models.AddAsync(item as CardModel);
            }
            catch (OperationCanceledException ex)
            {
                logger?.LogWarning($"[Sql] {ex.Message}");
            }
            db.SaveChanges();
        }

        public IEnumerable<TModel> Import<TModel>(object? options = null) where TModel : IModel => db.Models as IEnumerable<TModel>;

        public async Task Remove<TModel>(string guid) where TModel : IModel
        {
            logger.LogInformation($"[Sql] Remove entity | GUID : {guid}");
            var Card = await db.Models.FirstOrDefaultAsync(item => item.Id.ToString() == guid);
            db.Models.Remove(Card);
            db.SaveChanges();
        }

        public string DbPath
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

