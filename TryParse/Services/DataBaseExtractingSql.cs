using Microsoft.AspNetCore.Mvc;
using TryParse.Models;

namespace TryParse.Services
{
    public class DataBaseExtractingSql : IDataBaseExtracting
    {
        public async Task Export<TModel>(TModel entity, object? options = null) where TModel : IModel
        {
            throw new NotImplementedException();
        } 

        public async Task Export<TModel>(IEnumerable<TModel>? entity, object? options = null) where TModel : IModel
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Model> Import<Model>(object? options) where Model : IModel
        {
            throw new NotImplementedException();
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
