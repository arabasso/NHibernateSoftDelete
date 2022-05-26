using System;

namespace NHibernateSoftDelete.Models
{
    public class Comment :
        ISoftDeletable
    {
        public virtual long Id { get; set; }
        public virtual bool Deleted { get; set; }
        public virtual Article Article { get; set; }
        public virtual Author Author { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Text { get; set; }
    }
}