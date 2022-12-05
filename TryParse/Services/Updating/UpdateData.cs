using TryParse.Models;

namespace TryParse.Services.Updating
{
    public interface IUpdateData
    {
        public static IEnumerable<IModel> Models { get; set; }
        public List<IModel> Update<TModel>(string request) where TModel : IModel;
    }
}
