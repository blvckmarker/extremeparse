using ExtremeParse.Models;

namespace ExtremeParse.Services.Updating
{
    public sealed class SearchService : IUpdateData
    {
        private static IEnumerable<IModel> models = null!;

        public static IEnumerable<IModel> Models { get => models; set => models = value; }

        public List<IModel> Update<TModel>(string request) where TModel : IModel
        {
            request = request ?? string.Empty;
            return models.Where(model => model.ToString().Contains(string.Join("", request.Where(ch => ch != ' ')))).ToList();
        }

    }
}
