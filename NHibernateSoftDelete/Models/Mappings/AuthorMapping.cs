using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernateSoftDelete.Models.Mappings
{
    class AuthorMapping :
        ClassMapping<Author>
    {
        public AuthorMapping()
        {
            Table("Authors");
            Id(m => m.Id, p => p.Generator(Generators.Identity));
            Filter("NoDeleted", fm => fm.Condition("Deleted = '0'"));
            Property(m => m.Deleted);
            Property(m => m.Name);
            Bag(m => m.Comments, p =>
            {
                p.Inverse(true);
                p.Key(k => k.Column("AuthorId"));
                p.Cascade(Cascade.All);
                p.Filter("NoDeleted", fm => fm.Condition("Deleted = '0'"));
            }, mm => mm.OneToMany());
            Bag(m => m.Articles, p =>
            {
                p.Inverse(true);
                p.Key(k => k.Column("AuthorId"));
                p.Cascade(Cascade.All);
                p.Filter("NoDeleted", fm => fm.Condition("Deleted = '0'"));
            }, mm => mm.OneToMany());
        }
    }
}