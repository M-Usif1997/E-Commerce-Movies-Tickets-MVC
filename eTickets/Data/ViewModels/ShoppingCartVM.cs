using E_Commerce_Movies.Data.Cart;
using Microsoft.AspNetCore.SignalR;

namespace E_Commerce_Movies.Data.ViewModels
{
    public class ShoppingCartVM
    {

        public ShoppingCart ShoppingCart { get; set; }
        public double ShoppingCartTotal { get; set; }
        
    }
}
