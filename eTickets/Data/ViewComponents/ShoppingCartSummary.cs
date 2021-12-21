using E_Commerce_Movies.Data.Cart;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Movies.Data.ViewComponents
{
    // You can think of a VC as a mini-controller—it’s responsible for rendering a chunk rather than a whole response (Not depend on Contoller) & View components do not use model binding
    // it just a enhancement of partial view and another difference is when you use partial view you still have dependency on controller while in View Component you don't need a controller. So there is a separation of concern.
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public  IViewComponentResult Invoke()
        {

            var items =  _shoppingCart.GetShoppingCartItems();

            return View(items.Count);
        }
    }
}
