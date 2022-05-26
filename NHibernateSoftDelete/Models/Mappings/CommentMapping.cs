using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernateSoftDelete.Models.Mappings
{
    class CommentMapping :
        ClassMapping<Comment>
    {
        public CommentMapping()
        {
            Table("Comments");
            Id(m => m.Id, p => p.Generator(Generators.Identity));
            Filter("NoDeleted", fm => fm.Condition("Deleted = '0'"));
            Property(m => m.Deleted);
            Property(m => m.Date, p => p.Type(NHibernateUtil.Date));
            ManyToOne(m => m.Author, p =>
            {
                p.NotNullable(true);
                p.Column("AuthorId");
                p.ForeignKey("CommentAuthorFk");
            });
            ManyToOne(m => m.Article, p =>
            {
                p.NotNullable(true);
                p.Column("ArticleId");
                p.ForeignKey("CommentArticleFk");
            });
        }
    }
}