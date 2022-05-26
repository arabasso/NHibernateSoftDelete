using System.Collections.Generic;

namespace NHibernateSoftDelete.Models
{
    public class Author :
        ISoftDeletable
    {
        public virtual long Id { get; set; }
        public virtual bool Deleted { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Comment> Comments
        {
            get => _comments ??= new List<Comment>();
            set => _comments = value;
        }
        private IList<Comment> _comments;
        public virtual IList<Article> Articles
        {
            get => _articles ??= new List<Article>();
            set => _articles = value;
        }
        private IList<Article> _articles;
    }
}