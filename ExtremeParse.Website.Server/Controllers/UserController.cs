using ExtremeParse.Models;
using ExtremeParse.Services.Extracting;
using ExtremeParse.Services.Updating;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExtremeParse.Controllers
{
    public class UserController : Controller
    {
        #region Fields
        private IEnumerable<IModel> _cards;

        private IDataBaseExtracting dataBaseJson;
        private IDataBaseExtracting dataBaseSql;
        private event EventHandler OnCardModelChanged;

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


        [HttpPost]
        [Route("{controller}/api/tg/add")]
        public IActionResult CreateCard([FromBody] CardModel card)
        {
            if (card is null)
                return BadRequest();



            logger.LogInformation($"[{DateTime.Now}] - created new card (Telegram)");

            card.Id = Guid.NewGuid();
            card.Creator = "Telegram-Bot";
            card.DateTime = DateTime.Now;

            typeof(CardModel).GetProperties().ToList().ForEach(prop => { if (prop.GetValue(card) is null) prop.SetValue(card, "[I found nothing]"); });

            dataBaseSql.Export<CardModel>(card);
            return Ok();
        }


        [HttpPost]
        [Route("{controller}/api")]
        public IActionResult NewCard(CardModel newCard)
        {
            logger.LogInformation($"[{DateTime.Now}] - created new card");

            newCard.Id = Guid.NewGuid();
            newCard.Creator = "Parssa";
            newCard.DateTime = DateTime.Now;

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