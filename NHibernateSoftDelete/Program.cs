using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;
using NHibernateSoftDelete.Models;

namespace NHibernateSoftDelete
{
    class Program
    {
        static async Task Main()
        {
            var configuration = new Configuration()
                .DataBaseIntegration(db =>
                {
                    //db.Dialect<PostgreSQL83Dialect>();
                    //db.Driver<NpgsqlDriver>();
                    //db.ConnectionString = @"server=localhost;database=test;uid=test;pwd=test;";

                    //db.Dialect<MsSql2012Dialect>();
                    //db.Driver<Sql2008ClientDriver>();
                    //db.ConnectionString = @"server=localhost\SQLEXPRESS;database=test;uid=test;pwd=test;";

                    db.Dialect<MySQL57Dialect>();
                    db.Driver<MySqlConnectorDriver>();
                    db.ConnectionString = "server=localhost;database=test;uid=test;pwd=test;";

                    //db.Dialect<Oracle12cDialect>();
                    //db.Driver<OracleManagedDataClientDriver>();
                    //db.ConnectionString = "data source=localhost/XEPDB1;user id=test;password=test;";

                    db.SchemaAction = SchemaAutoAction.Create;
                    db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                });

            configuration.AddFilterDefinition(new FilterDefinition("NoDeleted", null, new Dictionary<string, IType>(), false));

            var modelMapper = new ModelMapper();

            modelMapper.AddMapping<Models.Mappings.ArticleMapping>();
            modelMapper.AddMapping<Models.Mappings.AuthorMapping>();
            modelMapper.AddMapping<Models.Mappings.CommentMapping>();

            configuration.AddMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities());
            configuration.SetListener(ListenerType.Delete, new SoftDeleteEventListener());

            using var sessionFactory = configuration.BuildSessionFactory();

            using var session = sessionFactory
                .WithOptions()
                .Interceptor(new SoftDeleteInterceptor())
                .OpenSession();

            var author1 = new Author
            {
                Name = "Zé",
            };

            await session.SaveAsync(author1);

            var author2 = new Author
            {
                Name = "Jão",
            };

            await session.SaveAsync(author2);
            await session.FlushAsync();

            var article1 = new Article
            {
                Title = "Lorem Ipsum",
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam quis justo lacus. Nam consequat congue tristique. In hac habitasse platea dictumst. In accumsan lacus vitae rhoncus malesuada. Nulla facilisi. Aliquam imperdiet volutpat nunc eu consectetur. Nulla rhoncus lobortis ligula, a scelerisque dui ornare non. Nulla quam arcu, egestas vitae massa at, imperdiet dapibus tellus. Curabitur eu turpis sed est bibendum gravida sit amet in arcu. Integer pulvinar nibh urna, nec porttitor ligula mattis sed. Aenean venenatis porta dui, a placerat libero semper nec. Integer id pretium tortor. Curabitur sed quam eleifend, dapibus enim non, ullamcorper eros. Donec vel consequat magna. Vestibulum ultrices sollicitudin ultrices.",
                Date = DateTime.Now.Date,
                Author = author1,
            };

            article1.Comments.Add(new Comment
            {
                Date = DateTime.Now.Date,
                Author = author2,
                Article = article1,
                Text = "Quisque tincidunt magna diam. Sed velit justo, malesuada ac lacus sed, mollis consectetur lectus. Integer eleifend in velit quis tincidunt. Quisque lobortis turpis congue tempor accumsan. Etiam consectetur consequat sem, eu convallis velit iaculis vitae. Maecenas feugiat condimentum nunc id sagittis. Maecenas ac venenatis lacus, quis auctor magna.",
            });

            await session.SaveAsync(article1);
            await session.FlushAsync();

            await session.DeleteAsync(article1);
            await session.FlushAsync();

            session.Clear();

            Console.WriteLine("* NoDeleted filter enabled");
            Console.WriteLine();

            foreach (var d in await session.Query<Article>().ToListAsync())
            {
                Console.WriteLine($"\t{d.Date:d} - {d.Author.Name} - {d.Title}");

                foreach (var c in d.Comments)
                {
                    Console.WriteLine($"\t\t{c.Author.Name} - {c.Date:d}");
                }
            }

            session.Clear();

            session.DisableFilter("NoDeleted");

            Console.WriteLine("* NoDeleted filter disabled");
            Console.WriteLine();

            foreach (var article in await session.Query<Article>().ToListAsync())
            {
                Console.WriteLine($"\t{article.Date:d} - {article.Author.Name} - {article.Title}");

                foreach (var comment in article.Comments)
                {
                    Console.WriteLine($"\t\t{comment.Author.Name} - {comment.Date:d}");
                }
            }

            // Removing permanently

            foreach (var article in await session.Query<Article>().Where(w => w.Deleted).ToListAsync())
            {
                await session.DeleteAsync(article);
                await session.FlushAsync();
            }

            session.Close();

            await sessionFactory.CloseAsync();
        }
    }
}
