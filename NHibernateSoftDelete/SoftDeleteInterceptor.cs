using NHibernate;

namespace NHibernateSoftDelete
{
    class SoftDeleteInterceptor :
        EmptyInterceptor
    {
        public override void SetSession(
            ISession session)
        {
            session.EnableFilter("NoDeleted");
        }
    }
}