using System.Collections.Generic;

namespace Lab10.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
        Category this[int id] { get; }
        Category AddCategory(Category category);
        Category UpdateCategory(Category category);
        void DeleteCategory(int id);
    }
}
