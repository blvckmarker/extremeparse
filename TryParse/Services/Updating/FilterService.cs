using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography.Xml;
using TryParse.Models;

namespace TryParse.Services.Updating
{
    public sealed class FilterService : IUpdateData
    {
        private static IEnumerable<IModel> models;
        public static IEnumerable<IModel> Models { get { return models; } set { models = value; } }

        public List<IModel>? Update<TModel>(string request) where TModel : IModel
        {
            //var pinfo = typeof(TModel).GetProperties();
            //models.OrderBy(models => pinfo.FirstOrDefault(x => x.Name == request).GetValue(models)) as IEnumerable<TModel>;
            //var props = models.Select(model => typeof(TModel).GetProperty(request).GetValue(model));
            return models.OrderBy(model => typeof(TModel).GetProperty(request).GetValue(model)).ToList();

        }
    }
}
