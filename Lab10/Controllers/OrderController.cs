using Lab10.Data;
using Lab10.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab10.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly Dictionary<int, string> payMethods = new Dictionary<int, string>(){
                {1, "Credit or debit card"},
                {2, "Blik"},
                {3, "Cash on delivery"}
            };

        public OrderController(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var articleList = await _context.Articles.ToListAsync();
            var keys = Request.Cookies.Keys.Where(k => k.StartsWith("art")).Select(k => Convert.ToInt32(k.Substring(3)));
            var list = GetCartResult(articleList, keys);
            
            return View(list);
        }

        //POST index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string firstName, string lastName, string address, string zipcode, string city, int payMethod)  //payMeth?? enum??
        {
            TempData["firstName"] = firstName;
            TempData["lastName"] = lastName;
            TempData["address"] = $"{address}, {city}, {zipcode}";
            TempData["payMethod"] = payMethods[payMethod];
            return RedirectToAction(nameof(Confirmation));
        }

        public IActionResult Confirmation()
        {
            clearCart();
            return View();
        }

        private IEnumerable<KeyValuePair<Article, int>> GetCartResult(List<Article> articleList, IEnumerable<int> keys)
        {
            var list = articleList.Join(keys,a => a.ArticleId, k => k,
                                        (a, k) => new KeyValuePair<Article, int>(a, int.Parse(Request.Cookies[$"art{k}"])));

            return list;
        }
        
        private void clearCart()
        {
            var keys = Request.Cookies.Keys.Where(k => k.StartsWith("art"));
            foreach(var key in keys)
            {
                Response.Cookies.Delete(key);
            }
        }
    }
}
