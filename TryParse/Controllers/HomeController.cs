using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TryParse.Models;
using TryParse.Services.Extracting;
using TryParse.Services.Updating;

namespace TryParse.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private static IEnumerable<IModel> _cards = default;
        private IDataBaseExtracting dataBase;
        private event EventHandler observer;

        private readonly SearchService searchService = new();
        private readonly FilterService filterService = new();
        private readonly ILogger<HomeController> _logger;

        #endregion
        public IEnumerable<IModel> Cards
        {
            get { return _cards; }
            set
            { 
                _cards = value;
                observer.Invoke(this, new EventArgs());
            }
        }
        public HomeController(DataBaseExtractingJson dataBaseExtracting, ILogger<HomeController> logger)
        {
            dataBase = dataBaseExtracting;
            _logger = logger;
            observer += OnDataChange;
        }

        private IEnumerable<CardModel> GetCardModels() => dataBase.Import<CardModel>(dataBase.DbPath);

        [HttpPost]
        [Route("{controller}/api")]
        public IActionResult NewCard(CardModel newCard)
        {
            _logger.Log(LogLevel.Information, $"[{DateTime.Now}] - created new card");
            newCard.Id = Guid.NewGuid();
            newCard.Creator = "Parssa";
            newCard.DateTime = DateTime.Now;
            dataBase.Export<CardModel>(newCard, dataBase.DbPath);
            return Redirect("/");
        }


        #region ServiceController
        [HttpPost]
        [Route("{controller}/api/filtercard")]
        public IActionResult FilterReq([FromQuery] string type)
        {
            _cards = filterService.Update<CardModel>(type);
            return Redirect("/");
        }


        [HttpPost]
        [Route("{controller}/api/searchcard")]
        public IActionResult SearchReq([FromQuery] string src)
        {
            _cards = searchService.Update<CardModel>(src);
            return Redirect("/");
        }
        #endregion

        private void OnDataChange(object? obj, EventArgs e) => FilterService.Models = SearchService.Models = _cards;
        public IActionResult Index() => View(Cards);
        public IActionResult Privacy() => View();


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, });
        }
    }
}