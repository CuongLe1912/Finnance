using Microsoft.EntityFrameworkCore;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF.Trackable;

namespace Meey.Admin.Repository
{
    public interface IRepositoryX<TEntity> : ITrackableRepository<TEntity> where TEntity : class, ITrackable
    {

    }

    public class RepositoryX<TEntity> : TrackableRepository<TEntity>, IRepositoryX<TEntity> where TEntity : class, ITrackable
    {
        public RepositoryX(DbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }
}