using TryParse.Models;

namespace TryParse.Services.Updating
{
    public sealed class SearchService : IUpdateData
    {
        private static IEnumerable<IModel> models;

        public static IEnumerable<IModel> Models { get { return models; } set { models = value; } }


        public List<IModel> Update<TModel>(string request) where TModel : IModel
        {
            throw new NotImplementedException();
        }
    }
}
