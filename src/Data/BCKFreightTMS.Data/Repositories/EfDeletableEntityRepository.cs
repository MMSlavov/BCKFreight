﻿namespace BCKFreightTMS.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Common.Models;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class EfDeletableEntityRepository<TEntity> : EfRepository<TEntity>, IDeletableEntityRepository<TEntity>
        where TEntity : class, IDeletableEntity, ICompanyEntity
    {
        public EfDeletableEntityRepository(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
            : base(context, httpContextAccessor, userManager)
        {
        }

        public override IQueryable<TEntity> All() => base.All().Where(x => !x.IsDeleted);

        public override IQueryable<TEntity> AllAsNoTracking() => base.AllAsNoTracking().Where(x => !x.IsDeleted);

        public IQueryable<TEntity> AllWithDeleted() => base.All().IgnoreQueryFilters();

        public IQueryable<TEntity> AllAsNoTrackingWithDeleted() => base.AllAsNoTracking().IgnoreQueryFilters();

        public Task<TEntity> GetByIdWithDeletedAsync(params object[] id)
        {
            var getByIdPredicate = EfExpressionHelper.BuildByIdPredicate<TEntity>(this.Context, id);
            return this.AllWithDeleted().FirstOrDefaultAsync(getByIdPredicate);
        }

        public void HardDelete(TEntity entity) => base.Delete(entity);

        public void Undelete(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;
            this.Update(entity);
        }

        public override void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            this.Update(entity);
        }
    }
}
