using System;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TrackableEntities.Common.Core;
using TrackableEntities.EF.Core;
using URF.Core.Abstractions.Trackable;

namespace URF.Core.EF.Trackable
{
    public class TrackableRepository<TEntity> : Repository<TEntity>, ITrackableRepository<TEntity>
        where TEntity : class, ITrackable
    {
        private readonly int _userId;
        public TrackableRepository(
            DbContext context,
            IHttpContextAccessor httpContextAccessor) : base(context)
        {
            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null)
            {
                var identity = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (identity != null)
                    _userId = Convert.ToInt32(identity.Value);
            }
        }

        public override void Insert(TEntity item)
        {
            var targetProprties = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in targetProprties)
            {
                if (property.Name == "IsActive")
                {
                    var valueProperty = property.GetValue(item);
                    if (valueProperty == null)
                        property.SetValue(item, true);
                }
                if (property.Name == "IsDelete")
                {
                    var valueProperty = property.GetValue(item);
                    if (valueProperty == null)
                        property.SetValue(item, false);
                }
                if (property.Name == "CreatedBy")
                {
                    var valueProperty = property.GetValue(item);
                    if (valueProperty == null && _userId != 0)
                        property.SetValue(item, _userId);
                }
                if (property.Name == "CreatedDate")
                {
                    var valueProperty = property.GetValue(item);
                    if (valueProperty == null)
                        property.SetValue(item, DateTime.Now);
                }
            }
            item.TrackingState = TrackingState.Added;
            base.Insert(item);
        }

        public override void Update(TEntity item)
        {

            var targetProprties = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in targetProprties)
            {
                if (property.Name == "UpdatedBy")
                {
                    var valueProperty = property.GetValue(item);
                    if (valueProperty == null && _userId != 0)
                        property.SetValue(item, _userId);
                }
                if (property.Name == "UpdatedDate")
                {
                    var valueProperty = property.GetValue(item);
                    property.SetValue(item, DateTime.Now);
                }
            }
            item.TrackingState = TrackingState.Modified;
            base.Update(item);
        }

        public override void Delete(TEntity item)
        {
            item.TrackingState = TrackingState.Deleted;
            base.Delete(item);
        }

        public override async Task<bool> DeleteAsync(object[] keyValues, CancellationToken cancellationToken = default)
        {
            var item = await FindAsync(keyValues, cancellationToken);
            if (item == null) return false;
            item.TrackingState = TrackingState.Deleted;
            Context.Entry(item).State = EntityState.Deleted;
            return true;
        }

        public virtual void ApplyChanges(params TEntity[] entities)
            => Context.ApplyChanges(entities);

        public virtual void AcceptChanges(params TEntity[] entities)
            => Context.AcceptChanges(entities);

        public virtual void DetachEntities(params TEntity[] entities)
            => Context.DetachEntities(entities);

        public virtual async Task LoadRelatedEntities(params TEntity[] entities)
            => await Context.LoadRelatedEntitiesAsync(entities);
    }
}
