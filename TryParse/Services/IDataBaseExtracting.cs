using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using TryParse.Models;

namespace TryParse.Services
{
    public interface IDataBaseExtracting
    {
        public IEnumerable<TModel> Import<TModel>(object? options) where TModel : IModel; //пути и тп
        public Task Export<TModel>(TModel entity, object? options = null) where TModel : IModel ; 
        public Task Export<TModel>(IEnumerable<TModel>? entity, object? options = null) where TModel : IModel;
        public string DbPath { get; }
    }
}
