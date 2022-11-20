using Lab10.Data;
using Lab10.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lab10.Controllers
{
    [Route("api/article/")]
    [ApiController]
    public class ArticleAPIController : ControllerBase
    {
        private readonly ShopDbContext _context;
        private IArticleRepository repository;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;

        public ArticleAPIController(ShopDbContext context, IArticleRepository repo, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            repository = repo;
            context.Articles.Include(a => a.Category).ToList().ForEach(a => repository.AddArticle(a));
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IEnumerable<Article> Get() => repository.Articles;

        [HttpGet("{id}")]
        public Article Get(int id) => repository[id];

        [HttpPost]
        [EnableCors]
        [Route("{categoryId},{name},{price}")]
        public async Task<Article> Post(int categoryId, string name, double price)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return null;
            }
            Article art = new Article() { CategoryId = categoryId, Category = category, Name = name, Price = price, FileName = "default-product.jpg" };
            _context.Add(art);
            await _context.SaveChangesAsync();
            var article = _context.Articles.Where(a => a.Name == art.Name && a.CategoryId == art.CategoryId).FirstOrDefault();
            return repository.AddArticle(article);
        }

        [HttpPut]
        public async Task<Article> Put([FromBody] Article res)
        {
            var article = await _context.Categories.FindAsync(res.ArticleId);
            if (article == null)
            {
                return null;
            }
            var category = await _context.Categories.FindAsync(res.CategoryId);
            if (category == null)
            {
                return null;
            }
            res.Category = category;
            res.FormFile = null;
            res.FileName = "default-product.jpg";
            _context.Update(res);
            await _context.SaveChangesAsync();
            return repository.UpdateArticle(res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound($"Article with id = {id} not found");
            }
            string deleteFolder = Path.Combine(_hostingEnvironment.WebRootPath, "upload\\") + article.FileName;
            if (System.IO.File.Exists(deleteFolder))
            {
                System.IO.File.Delete(deleteFolder);
            }
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            repository.DeleteArticle(id);
            return Ok();
            
        }

        [HttpGet("next/{id}/{catId}")]
        public IEnumerable<Article> GetNextN(int id, int catId) => repository.GetNextN(id, catId);
    }
}
