using System.Collections.Generic;

namespace DrinksVendorMachine.Models.Repository
{
    public class Repository
    {
        private readonly EfDbContext _context = new EfDbContext();

        public IEnumerable<Drink> Drinks // таблица напитков
        {
            get { return _context.Drinks; }
        }

        public IEnumerable<Coin> Coins // таблица монет
        {
            get { return _context.Coins; }
        }

        public void SaveDrink(Drink drink) // изменения в таблице напитков
        {
            if (drink.DrinkId == 0)
            {
                _context.Drinks.Add(drink); // добавление новой записи
            }
            else
            {
                Drink dbDrink = _context.Drinks.Find(drink.DrinkId);
                if (dbDrink != null) // перезапись существующих данных
                {
                    dbDrink.Name = drink.Name;
                    dbDrink.Price = drink.Price;
                    dbDrink.Quantity = drink.Quantity;
                    dbDrink.ImageData = drink.ImageData;
                    dbDrink.ImageMimeType = drink.ImageMimeType;
                }
            }
            
            _context.SaveChanges(); // сохранение БД
        }

        public void SaveCoins(Coin coin) // то же для таблицы монет
        {
            if (coin.CoinId == 0)
            {
                _context.Coins.Add(coin);
            }
            else
            {
                Coin dbCoin = _context.Coins.Find(coin.CoinId);
                if (dbCoin != null)
                {
                    dbCoin.Nominal = coin.Nominal;
                    dbCoin.Quantity = coin.Quantity;
                    dbCoin.IsBlocked = coin.IsBlocked;
                }
            }

            _context.SaveChanges();
        }

        public void DeleteDrink(Drink drink) // удаление записей в напитках, для монет не предусмотрено
        {
            _context.Drinks.Remove(drink);
            _context.SaveChanges();
        }
    }
}