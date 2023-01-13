using ExtremeParse.Models;

namespace ExtremeParse.Services.Updating
{
    public interface IUpdateData
    {
        public static IEnumerable<IModel> Models { get; set; } = null!;
        public List<IModel> Update<TModel>(string request) where TModel : IModel;
    }
}
