using TryParse.Models;

namespace TryParse.Services.Updating
{
    public sealed class FilterService : IUpdateData
    {
        private static IEnumerable<IModel> models = null!;
        public static IEnumerable<IModel> Models { get => models; set => models = value; }

        public List<IModel>? Update<TModel>(string request) where TModel : IModel =>
            models.OrderBy(model => request switch
            {
                "Name" => model.Name,
                "Tg" => (model.Creator == "Telegram-Bot").ToString(),
                "Parssa" => (model.Creator == "Parssa").ToString(),
                _ => null
            }).ToList();
    }
}
