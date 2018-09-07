using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DrinksVendorMachine.Models;
using DrinksVendorMachine.Models.Repository;

namespace DrinksVendorMachine.Controllers
{

    public class HomeController : Controller
    {
        public static readonly Repository Repository = new Repository();
        public static string Message = "Введите монетки";
        public static int Sum;
        public static readonly Dictionary<int, int> Coins = new Dictionary<int, int>();           // 2 модели в одной вьюшке подключить нельзя (?),
        public static  readonly Dictionary<int, bool> CoinsBlocked = new Dictionary<int, bool>(); // поэтому стат. переменные - "слепок" таблицы с монетками
        
        static HomeController()
        {
            foreach (var coin in Repository.Coins)
            {
                Coins[coin.Nominal] = coin.Quantity; // здесь будут количества монет конкретного достоинства
                CoinsBlocked[coin.Nominal] = coin.IsBlocked; // данные по блокированным монетам
            }
        }
        
        //==============================================================================

        [HttpGet]
        public ActionResult Index()
        {
            return View(Repository.Drinks);
        }
       
        //==============================================================================

        [HttpGet]
        public ActionResult DropCoin(int nominal) // метод для "приёма" монет определённого достоинства
        {
            Coin coin = Repository.Coins.FirstOrDefault(x => x.Nominal == nominal); // нашли монетку
            if (coin != null && coin.IsBlocked == false)
            {
                coin.Quantity++; // увеличиваем количество монет данного номинала в базе
                Coins[nominal]++; // инкрементируем значение в стат. переменной
                Repository.SaveCoins(coin); // сохраняем изменения в БД
                Sum += nominal; // увеличиваем сумму для отображения во View на значение номинала
            }
            
            return RedirectToAction("Index");
        }
        //=============================================================================

        [HttpGet]
        public ActionResult ChooseDrink(int? drinkId) // действие выбора напитка пользователем
        {
            Drink drink = Repository.Drinks.FirstOrDefault(d => d.DrinkId == drinkId);

            if (drink == null) return RedirectToAction("Index");

            drink.Quantity--; // Уменьшаем количество напитка в базе
            Repository.SaveDrink(drink); // Сохраняем изменения в базе
            Sum -= drink.Price; // Превращаем сумму в сдачу
            Message = "Возьмите напиток и сдачу";

            return RedirectToAction("Index");
        }
        //==============================================================================

        [HttpGet]
        public ActionResult OddMoney() // обработка нажатия кнопки "забрать сдачу"
        {
            ReturnOddMoney(); // Возвращаем сдачу
            Sum = 0;
            Message = "Введите монетки";
            
            return RedirectToAction("Index");
        }
        //==============================================================================

        public void ReturnOddMoney() // декремент значений количества монет в БД при расчёте сдачи
        {
            int rest = Sum;
            List<int> nominal = CoinsBlocked.Keys.ToList(); // будем вычитать номинал монетки из остаточной суммы

            while (rest > 0)
            {
                for (int i = 3; i >= 0; i--) // начинаем сдачу "10-ками"
                {
                    Coin coin = Repository.Coins.FirstOrDefault(x => x.Nominal == nominal[i]); // получаем монетку

                    while (rest >= nominal[i])
                    {
                        rest -= nominal[i];
                        if (coin != null && coin.Quantity > 0)
                        {
                            coin.Quantity--; // "возвращаем" некую часть монет данного номинала, вычитаем её из БД
                            Coins[coin.Nominal]--; // не забудем продублировать информацию в статическую переменную
                            Repository.SaveCoins(coin); // сохраняем данные в БД
                        }
                        else if (coin != null && coin.Quantity == 0)
                        {
                            Message = "Извините, нет сдачи"; // Не выведется, надо использовать javascript на клиенте (?)
                        }
                    }
                }
            }
        }
    }
}
