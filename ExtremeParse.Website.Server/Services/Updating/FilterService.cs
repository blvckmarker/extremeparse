using ExtremeParse.Models;

namespace ExtremeParse.Services.Updating
{
    public sealed class FilterService : IUpdateData
    {
        private static IEnumerable<IModel> models = null!;
        public static IEnumerable<IModel> Models { get => models; set => models = value; }

        public List<IModel>? Update<TModel>(string request) where TModel : IModel =>
            models.OrderBy(model => request switch
            {
                "Name" => model.Name,
                "Parssa" => model.Creator,
                "Tb" => model.Creator == "Telegram-Bot" ? "Parssa" : "Telegram-Bot",
                "Date" => (-model.DateTime.Ticks).ToString(),
                _ => null
            }).ToList();
    }
}
