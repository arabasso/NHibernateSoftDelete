using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernateSoftDelete.Models.Mappings
{
    class ArticleMapping :
        ClassMapping<Article>
    {
        public ArticleMapping()
        {
            Table("Articles");
            Id(m => m.Id, p => p.Generator(Generators.Identity));
            Filter("NoDeleted", fm => fm.Condition("Deleted = '0'"));
            Property(m => m.Deleted);
            Property(m => m.Date, p => p.Type(NHibernateUtil.Date));
            Property(m => m.Title, p => p.Length(255));
            Property(m => m.Text, p => p.Length(1048576));
            ManyToOne(m => m.Author, p =>
            {
                p.NotNullable(true);
                p.Column("AuthorId");
                p.ForeignKey("ArticleAuthorFk");
            });
            Bag(m => m.Comments, p =>
            {
                p.Inverse(true);
                p.Key(k => k.Column("ArticleId"));
                p.Cascade(Cascade.All);
                p.Filter("NoDeleted", fm => fm.Condition("Deleted = '0'"));

            }, mm => mm.OneToMany());
        }
    }
}