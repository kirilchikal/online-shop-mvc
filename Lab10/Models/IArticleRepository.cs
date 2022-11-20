using System.Collections.Generic;

namespace Lab10.Models
{
    public interface IArticleRepository
    {
        IEnumerable<Article> Articles { get; }
        Article this[int id] { get; }
        Article AddArticle(Article article);
        Article UpdateArticle(Article article);
        void DeleteArticle(int id);
        IEnumerable<Article> GetNextN(int id, int catId);
    }
}
