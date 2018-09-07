using System.ComponentModel.DataAnnotations;

namespace DrinksVendorMachine.Models
{
    public class Coin
    {
        public int CoinId { get; set; }
        public int Nominal { get; set; }

        [Range(0, 250)] // условный лимит принимаемого количества монет
        public int Quantity { get; set; }

        public bool IsBlocked { get; set; }
    }
}