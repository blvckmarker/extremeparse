using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Diagnostics;
using TryParse.Models;
using TryParse.Services;

namespace TryParse.Controllers
{
    public class HomeController : Controller
    {

        private static IEnumerable<IModel> _cards = default;
        private IDataBaseExtracting dataBase;

        public IEnumerable<IModel> Cards
        {
            get
            {
                GetCardModels();
                return _cards;
            }
            set
            {
                _cards = value;
            }
        } 
        private void GetCardModels() => _cards = dataBase.Import<CardModel>(dataBase.DbPath);

        [HttpPost]
        [Route("{controller}/api")]
        public RedirectResult NewCard(CardModel newCard)
        {
            newCard.Id = Guid.NewGuid();
            newCard.Creator = "Parssa";
            newCard.DateTime = DateTime.Now;
            dataBase.Export<CardModel>(newCard, dataBase.DbPath);
            return Redirect("/");
        }

        //private readonly ILogger<HomeController> _logger;

        public HomeController(DataBaseExtractingJson dataBaseExtracting)
        {
            dataBase = dataBaseExtracting;
            //_logger = logger;
        }

        public IActionResult Index()
        {
            return View(Cards);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}