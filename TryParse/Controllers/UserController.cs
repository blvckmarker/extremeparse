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

        private IDataBaseExtracting dataBaseJson = new DataBaseExtractingJson();
        private IDataBaseExtracting dataBaseSql = new DataBaseExtractingSql();
        private event EventHandler OnCardModelChanged;


        private readonly SearchService searchService = new();
        private readonly FilterService filterService = new();
        private readonly ILogger<UserController> _logger;


        public UserController()
        {
            OnCardModelChanged += OnDataChange;
        }

        #endregion
        public IEnumerable<IModel> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                OnCardModelChanged.Invoke(this, new EventArgs());
            }
        }

        private IEnumerable<IModel> GetCardsModel() => dataBaseJson.Import<CardModel>(dataBaseJson.DbPath);

        [HttpPost]
        [Route("{controller}/api")]
        public IActionResult NewCard(CardModel newCard)
        {
            //_logger.Log(LogLevel.Information, $"[{DateTime.Now}] - created new card");

            newCard.Id = Guid.NewGuid();
            newCard.Creator = "Parssa";
            newCard.DateTime = DateTime.Now;

            dataBaseJson.Export<CardModel>(newCard, dataBaseJson.DbPath);
            Cards = GetCardsModel();

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
                _ => GetCardsModel()
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