using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrinksVendorMachine.Models;
using DrinksVendorMachine.Models.Repository;

namespace DrinksVendorMachine.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly Repository _repository = HomeController.Repository; // ссылка на стат. хранилище
        //==================================================================

        [HttpGet]
        public ActionResult Admin()
        {
            return View(_repository.Drinks);
        }
        //==================================================================

        [HttpPost]//[HttpPost, ValidateInput(false)]
        public ActionResult EditDrink(Drink drink, HttpPostedFileBase image = null)
        {
            Drink oldDrink = _repository.Drinks.FirstOrDefault(d => d.DrinkId == drink.DrinkId); // нашли напиток

            if (ModelState.IsValid)
            {
                if (image != null) // читаем изображение
                {
                    drink.ImageMimeType = image.ContentType;
                    drink.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(drink.ImageData, 0, image.ContentLength);
                }
                else // сохраняем в картинку её самоё, иначе в БД пойдёт null (если нажали "добавить" и не выбрали ничего)
                {
                    if (oldDrink != null)
                    {
                        drink.ImageData = oldDrink.ImageData;
                        drink.ImageMimeType = oldDrink.ImageMimeType;
                    }
                }
            }
            
            if (oldDrink != null) // перезаписываем данные в экземпляре модели
            {
                oldDrink.DrinkId = drink.DrinkId;
                oldDrink.Name = drink.Name;
                oldDrink.Price = drink.Price;
                oldDrink.Quantity = drink.Quantity;
                oldDrink.ImageData = drink.ImageData;
                oldDrink.ImageMimeType = drink.ImageMimeType;
            }
            _repository.SaveDrink(oldDrink); // сохраняем в БД эту запись

            return RedirectToAction("Admin");
        }
        //==================================================================

        [HttpPost]//, ValidateInput(false)
        public ActionResult AddDrink(Drink drink, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null) // читаем принятую картинку
                {
                    drink.ImageMimeType = image.ContentType;
                    drink.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(drink.ImageData, 0, image.ContentLength);
                }

                _repository.SaveDrink(new Drink
                {
                    DrinkId = drink.DrinkId,
                    Name = drink.Name,
                    Price = drink.Price,
                    Quantity = drink.Quantity,
                    ImageData = drink.ImageData,
                    ImageMimeType = drink.ImageMimeType
                }); // создаём экземпляр модели по полученным данным и сохраняем в БД
            }
            else
            {
                return RedirectToAction("Admin");
            }

            return RedirectToAction("Admin");
        }
        //==================================================================

        [HttpGet]
        public ActionResult DeleteDrink(int drinkId)
        {
            Drink drink = _repository.Drinks.FirstOrDefault(d => d.DrinkId == drinkId); // находим запись по id
            if (drink != null)
            {
                _repository.DeleteDrink(drink); // ...и удаляем её
            }

            return RedirectToAction("Admin");
        }
        //==================================================================

        [HttpGet]
        public ActionResult ResetCoins(int nominal) // метод сброса к-ва монет ("забрать кассу")
        {
            Coin coin = _repository.Coins.FirstOrDefault(c => c.Nominal == nominal);
            if (coin != null)
            {
                HomeController.Coins[nominal] = 0;
                coin.Quantity = 0;
                _repository.SaveCoins(coin);
            }

            return RedirectToAction("Admin");
        }
        //==================================================================

        [HttpGet]
        public ActionResult SetCoinNumber(int nominal, int quantity) // задать монете её количество (на сдачу, например)
        {
            Coin coin = _repository.Coins.FirstOrDefault(c => c.Nominal == nominal);
            if (coin != null)
            {
                HomeController.Coins[nominal] = quantity;
                coin.Quantity = quantity;
                _repository.SaveCoins(coin);
            }

            return RedirectToAction("Admin");
        }
        //==================================================================

        [HttpGet]
        public ActionResult BlockCoin(int nominal) // блокировка монет
        {
            Coin coin = _repository.Coins.FirstOrDefault(c => c.Nominal == nominal);
            if (coin != null)
            {
                if (HomeController.CoinsBlocked[nominal] == false)
                {
                    HomeController.CoinsBlocked[nominal] = true;
                    coin.IsBlocked = true;
                }
                else
                {
                    HomeController.CoinsBlocked[nominal] = false;
                    coin.IsBlocked = false;
                }

                _repository.SaveCoins(coin);
            }

            return RedirectToAction("Admin");
        }
    }
}