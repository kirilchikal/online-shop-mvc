using Lab10.Data;
using Lab10.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab10.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopDbContext _context;

        public ShopController(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            ViewBag.Cat = 0;
            var categories = await _context.Categories.ToListAsync();
            categories.Insert(0, new Category() { CategoryId = 0, Name = "All" });
            var articles = await _context.Articles.ToListAsync();

            ShopViewModel shop = new ShopViewModel { Categories = categories, Articles = articles };

            if (categoryId != null && categoryId > 0)
            {
                shop.Articles = articles.Where(a => a.CategoryId == categoryId);
                ViewBag.Cat = categoryId;
            }
            shop.Articles.OrderBy(a => a.ArticleId);
            return View(shop);
        }

        public async Task<IActionResult> Cart()
        {
            var articleList = await _context.Articles.ToListAsync();
            var keys = Request.Cookies.Keys.Where(k => k.StartsWith("art")).Select(k => Convert.ToInt32(k.Substring(3)));
            var list = GetCartResult(articleList, keys);

            if (keys.Count() != list.Count())
            {
                var notFound = keys.Where(k => !list.Any(x => x.Key.ArticleId == k));
                string msg = notFound.Count() == 1 ? "Article has been deleted: " : "Articles have been deleted: ";
                foreach (var a in notFound)
                {
                    Response.Cookies.Delete($"art{a}");
                    msg += a.ToString() + " ";
                }
                TempData["ErrorMSG"] = msg;
            }
            
            return View(list);
        }


        public IActionResult AddToCart(int id, int? catId)
        {
            string key = $"art{id}";

            string count;
            Request.Cookies.TryGetValue(key, out count);
            if (count == null)
            {
                SetCookie(key, "1");
            }
            else
            {
                SetCookie(key, (int.Parse(count) + 1).ToString());
            }
            return RedirectToAction(nameof(Index), new { categoryId = catId});
        }


        public IActionResult AddItem(int? id)
        {
            string key = $"art{id}";
            string count;
            Request.Cookies.TryGetValue(key, out count);

            SetCookie(key, (int.Parse(count) + 1).ToString());

            return RedirectToAction(nameof(Cart));
        }

        public IActionResult RemoveItem(int? id)
        {
            string key = $"art{id}";
            string count;
            Request.Cookies.TryGetValue(key, out count);
            if (int.Parse(count) == 1)
            {
                Response.Cookies.Delete(key);
            }
            else
            {
                SetCookie(key, (int.Parse(count) - 1).ToString());
            }

            return RedirectToAction(nameof(Cart));
        }


        public IActionResult DeleteFromCart(int? id)
        {
            string key = $"art{id}";
            Response.Cookies.Delete(key);

            return RedirectToAction(nameof(Cart));
        }


        public void SetCookie(string key, string value)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append(key, value, options);
        }


        public IEnumerable<KeyValuePair<Article,int>> GetCartResult(List<Article> articleList, IEnumerable<int> keys)
        {
            var list = articleList.Join(keys,
                                        a => a.ArticleId,
                                        k => k,
                                        (a, k) => new KeyValuePair<Article, int>(a, int.Parse(Request.Cookies[$"art{k}"])));

            return list;
        }
    }
}
