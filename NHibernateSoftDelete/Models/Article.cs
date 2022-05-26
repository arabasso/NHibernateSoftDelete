using System;
using System.Collections.Generic;

namespace NHibernateSoftDelete.Models
{
    public class Article :
        ISoftDeletable
    {
        public virtual long Id { get; set; }
        public virtual bool Deleted { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Author Author { get; set; }
        public virtual string Title { get; set; }
        public virtual string Text { get; set; }

        public virtual IList<Comment> Comments
        {
            get => _comments ??= new List<Comment>();
            set => _comments = value;
        }
        private IList<Comment> _comments;
    }
}