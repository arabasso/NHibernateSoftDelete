using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Persister.Entity;
using NHibernateSoftDelete.Models;

namespace NHibernateSoftDelete
{
    public class SoftDeleteEventListener :
        DefaultDeleteEventListener
    {
        protected override void DeleteEntity(
            IEventSource session,
            object entity,
            EntityEntry entityEntry,
            bool isCascadeDeleteEnabled,
            IEntityPersister persister,
            ISet<object> transientEntities)
        {
            if (entity is ISoftDeletable { Deleted: false } o)
            {
                o.Deleted = true;

                CascadeBeforeDelete(session, persister, o, entityEntry, transientEntities);
                CascadeAfterDelete(session, persister, o, transientEntities);

            }

            else
            {
                base.DeleteEntity(session, entity, entityEntry, isCascadeDeleteEnabled, persister, transientEntities);
            }
        }

        protected override async Task DeleteEntityAsync(
            IEventSource session,
            object entity,
            EntityEntry entityEntry,
            bool isCascadeDeleteEnabled,
            IEntityPersister persister,
            ISet<object> transientEntities,
            CancellationToken cancellationToken)
        {
            if (entity is ISoftDeletable { Deleted: false } o)
            {
                o.Deleted = true;

                await CascadeBeforeDeleteAsync(session, persister, o, entityEntry, transientEntities, cancellationToken);
                await CascadeAfterDeleteAsync(session, persister, o, transientEntities, cancellationToken);

            }

            else
            {
                await base.DeleteEntityAsync(session, entity, entityEntry, isCascadeDeleteEnabled, persister, transientEntities, cancellationToken);
            }
        }
    }
}