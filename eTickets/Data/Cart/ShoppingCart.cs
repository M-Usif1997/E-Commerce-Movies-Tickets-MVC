using E_Commerce_Movies.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Commerce_Movies.Data.Cart
{
    public class ShoppingCart
    {
       

        public AppDbContext _context { get; set; }

        public string ShoppingCartId { get; set; }   
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(AppDbContext context)
        {       
            _context = context;
        }

        public ShoppingCart()
        {
        }

     
        public static ShoppingCart  GetShoppingCart(IServiceProvider service)
        {
            //Session stores the data in the dictionary on the Server and SessionId is used as a key
            //By default, SessionID values are stored in a cookie.
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;        /*access HttpContext inside the service class and ? => not Returns null if there is no active Microsoft.AspNetCore.Http.IHttpContextAccessor.HttpContext must be get */
            
            var context = service.GetService<AppDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();     //Stored in database by unique values NewGuid()  if null
            session.SetString("CartId", cartId);

         
                

            return new ShoppingCart(context) { ShoppingCartId = cartId };    //ShoppingChart Variable Contain now unique value for the entered user in website (Retrieve data from the cookies)
        }

        public void AddItemToCart(Movie movie)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(sc => sc.Movie.Id == movie.Id && sc.ShoppingCartId == ShoppingCartId);  //every item belong to a cart => ShoppingCartId   

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,                                      
                    Movie = movie,
                    Amount = 1
                };

                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _context.SaveChanges();
        }



        public void RemoveItemFromCart(Movie movie)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(sc => sc.Movie.Id == movie.Id && sc.ShoppingCartId == ShoppingCartId);  //every item has unique value of ShoppingCartId as i refere to specific user  

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;

                }

                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }

            }
           
            _context.SaveChanges();
        }

        

        public List<ShoppingCartItem> GetShoppingCartItems()   
        {
            return ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems.Where(sc => sc.ShoppingCartId == ShoppingCartId).Include(sc => sc.Movie).ToList());
        }

        public double GetShoppingCartTotal() => _context.ShoppingCartItems.Where(sc => sc.ShoppingCartId == ShoppingCartId).Select(sc => sc.Movie.Price * sc.Amount).Sum();


        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync(); 
            _context.ShoppingCartItems.RemoveRange(items);    //Remove Itmes in CartShopping For the User
            await _context.SaveChangesAsync();
        }
    }
}
