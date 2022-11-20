using Microsoft.AspNetCore.Http;
using Lab10.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab10.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace Lab10.Controllers
{
    [EnableCors]
    [Route("api/category/")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        private ICategoryRepository repository;
        private readonly ShopDbContext _context;

        public CategoryAPIController(ShopDbContext context, ICategoryRepository repo)
        {
            _context = context;
            repository = repo;
            context.Categories.ToList().ForEach(c =>repository. AddCategory(c));
        }

        [HttpGet]
        public IEnumerable<Category> Get() => repository.Categories;

        [HttpGet("{id}")]
        public Category Get(int id) => repository[id];

        [HttpPost]
        [EnableCors]
        public async Task<Category> Post([FromBody] Category res)
        {
            _context.Add(res);
            await _context.SaveChangesAsync();
            int key = _context.Categories.Where(c => c.Name == res.Name).Select(c => c.CategoryId).FirstOrDefault();
            return repository.AddCategory(new Category { CategoryId = key, Name = res.Name});
        }

        [HttpPut]
        public async Task<Category> Put([FromBody] Category res)
        {
            var category = await _context.Categories.FindAsync(res.CategoryId);
            if (category != null)
            {
                category.Name = res.Name;
                _context.Update(category);
                await _context.SaveChangesAsync();
            }

            return repository.UpdateCategory(res);
        }

        [HttpDelete("{id}")]  
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Category with id = {id} not found");
            }
            // delete category in context (what if there are articles in this category)
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.CategoryId == id);
            if (article == null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Deletion failed because of articles in category with id = {id}");
            }
            
            repository.DeleteCategory(id);
            return Ok();

        }
    }
}
