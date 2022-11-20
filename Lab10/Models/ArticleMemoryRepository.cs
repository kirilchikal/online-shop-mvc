using System.Collections.Generic;
using System.Linq;

namespace Lab10.Models
{
    public class ArticleMemoryRepository : IArticleRepository
    {
        private readonly Dictionary<int, Article> items;

        public ArticleMemoryRepository()
        {
            items = new Dictionary<int, Article>();
        }
        public Article this[int id] => items.ContainsKey(id) ? items[id] : null;

        public IEnumerable<Article> Articles => items.Values;

        public Article AddArticle(Article article)
        {
            items[article.ArticleId] = article;
            return article;
        }

        public void DeleteArticle(int id) => items.Remove(id);


        public IEnumerable<Article> GetNextN(int id, int catId)
        {
            if (catId != 0)
            {
                return items.Select(i => i.Value)
                .Where(a => a.ArticleId > id && a.CategoryId == catId)
                .OrderBy(a => a.ArticleId)
                .Take(3);
            }
            else return items.Select(i => i.Value)
                .Where(a => a.ArticleId > id)
                .OrderBy(a => a.ArticleId)
                .Take(3);
        }

        public Article UpdateArticle(Article category) => AddArticle(category);
    }
}
