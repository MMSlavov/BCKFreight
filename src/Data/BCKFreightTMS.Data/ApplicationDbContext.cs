namespace BCKFreightTMS.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Common.Models;
    using BCKFreightTMS.Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ActionType> ActionTypes { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<BankDetails> BankDetails { get; set; }

        public DbSet<Cargo> Cargos { get; set; }

        public DbSet<CargoType> CargoTypes { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyAddress> CompanyAddresses { get; set; }

        public DbSet<CompanyContact> CompanyContacts { get; set; }

        public DbSet<Comunicators> Comunicators { get; set; }

        public DbSet<DriverOrder> DriverOrders { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderAction> OrderActions { get; set; }

        public DbSet<OrderFrom> OrderFroms { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public DbSet<OrderTo> OrderTos { get; set; }

        public DbSet<OrderType> OrderTypes { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<PersonRole> PersonRoles { get; set; }

        public DbSet<TaxCountry> TaxCountries { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<VehicleLoadingBody> VehicleLoadingBodies { get; set; }

        public DbSet<VehicleType> VehicleTypes { get; set; }

        public DbSet<ActionNotFinishedReason> ActionNotFinishedReasons { get; set; }

        public DbSet<Documentation> Documentations { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<InvoiceIn> InvoiceIns { get; set; }

        public DbSet<InvoiceIn> InvoiceOuts { get; set; }

        public DbSet<CarrierOrder> CarrierOrders { get; set; }

        public DbSet<InvoiceStatus> InvoiceStatuses { get; set; }

        public DbSet<VATReason> VATReasons { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
