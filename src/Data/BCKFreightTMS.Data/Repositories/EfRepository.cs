namespace BCKFreightTMS.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Models;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, ICompanyEntity
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public EfRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.DbSet = this.Context.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected ApplicationDbContext Context { get; set; }

        public virtual IQueryable<TEntity> All() => this.GetSet(this.DbSet);

        public virtual IQueryable<TEntity> AllAsNoTracking() => this.GetSet(this.DbSet.AsNoTracking());

        public virtual Task AddAsync(TEntity entity)
        {
            var user = this.User().Result;
            entity.AdminId = user.AdminId;

            return this.DbSet.AddAsync(entity).AsTask();
        }

        public virtual void Update(TEntity entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity) => this.DbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Context?.Dispose();
            }
        }

        private IQueryable<TEntity> GetSet(IQueryable<TEntity> dbSet)
        {
            return this.IsUserAdmin() ?
                    dbSet :
                    dbSet.Where(e => e.AdminId == this.User().Result.AdminId || e.AdminId == null);
        }

        private bool IsUserAdmin()
        {
            return this.httpContextAccessor.HttpContext.User.IsInRole(RoleNames.Admin.ToString());
        }

        private async Task<ApplicationUser> User()
        {
            return await this.userManager.GetUserAsync(this.httpContextAccessor.HttpContext.User);
        }
    }
}
