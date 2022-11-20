using Lab10.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab10.Models
{
    public class CategoryMemoryRepository : ICategoryRepository
    {
        private readonly Dictionary<int, Category> items;

        public CategoryMemoryRepository()
        {
            items = new Dictionary<int, Category>();
            //context.Categories.ToList().ForEach(c => AddCategory(c));
        }
        public Category this[int id] => items.ContainsKey(id) ? items[id] : null;

        public IEnumerable<Category> Categories => items.Values;

        public Category AddCategory(Category category)
        {
            items[category.CategoryId] = category;
            return category;
        }

        public void DeleteCategory(int id) => items.Remove(id);

        public Category UpdateCategory(Category category) => AddCategory(category);
    }
}
