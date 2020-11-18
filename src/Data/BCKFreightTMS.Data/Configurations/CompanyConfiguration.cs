namespace BCKFreightTMS.Data.Configurations
{
    using BCKFreightTMS.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> company)
        {
            company
                .HasOne(c => c.Address)
                .WithOne(a => a.Company)
                .HasForeignKey<Company>(c => c.CompanyAddressId);

            company
                .HasOne(c => c.Comunicators)
                .WithOne(com => com.Company)
                .HasForeignKey<Company>(c => c.ComunicatorsId);
        }
    }
}
