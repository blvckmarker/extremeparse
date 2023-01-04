using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TryParse.Models;
using TryParse.Services.Extracting;
using TryParse.Services.Updating;

namespace TryParse.Controllers
{
    public class UserController : Controller
    {
        #region Fields
        private IEnumerable<IModel> _cards;

        private IDataBaseExtracting dataBaseJson;
        private IDataBaseExtracting dataBaseSql;
        private event EventHandler OnCardModelChanged;
        private static int order = 0;

        private readonly SearchService searchService = new();
        private readonly FilterService filterService = new();
        private readonly ILogger logger;

        private IEnumerable<IModel> GetCardsModel() => dataBaseSql.Import<CardModel>(dataBaseJson.DbPath);
        public UserController(ILogger<UserController> logger)
        {
            this.logger = logger;
            logger.LogInformation("[User] Create controller");

            dataBaseSql = new DataBaseExtractingSql(logger);
            dataBaseJson = new DataBaseExtractingJson(logger);

            OnCardModelChanged += OnDataChange;
        }

        #endregion
        public IEnumerable<IModel> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                logger.LogInformation($"Set new value of cards - [{value.Count()} cards]");
                OnCardModelChanged.Invoke(this, new EventArgs());
            }
        }

        //{fa042b12-4053-4947-900a-ebdb275f0dcc} 1
        //{c3ff6af9-c91a-40e9-9ea7-7ab56f277357} 2

        [HttpPost]
        [Route("{controller}/api")]
        public IActionResult NewCard(CardModel newCard)
        {
            logger.LogInformation($"[{DateTime.Now}] - created new card");

            newCard.Id = Guid.NewGuid();
            newCard.Creator = "Telegram-Bot";
            newCard.DateTime = DateTime.Now;

            order++;
            dataBaseSql.Export<CardModel>(newCard);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("{controller}/api/rm")]
        public IActionResult RemoveCard([FromQuery] string guid)
        {
            logger.LogInformation($"[{DateTime.Now}] - removed card {guid}");
            dataBaseSql.Remove<CardModel>(guid);
            return RedirectToAction("Index");
        }


        #region ServiceController
        [HttpPost]
        [Route("{controller}/api/filtercard")]
        public IActionResult FilterReq([FromQuery] string type) => RedirectToAction("Index", new { request = "filter", type = type });

        [HttpPost]
        [Route("{controller}/api/searchcard")]
        public IActionResult SearchReq([FromQuery] string src) => RedirectToAction("Index", new { request = "search", type = src });

        #endregion

        private void OnDataChange(object? obj, EventArgs e) => FilterService.Models = SearchService.Models = _cards;
        public IActionResult Index(string request, string type)
        {
            Cards = request switch
            {
                "filter" => filterService.Update<CardModel>(type),
                "search" => searchService.Update<CardModel>(type),
                _ => GetCardsModel().OrderByDescending(item => item.DateTime.Ticks)
            };

            return View(Cards);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, });
        }
    }
}