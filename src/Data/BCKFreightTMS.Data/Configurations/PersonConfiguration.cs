namespace BCKFreightTMS.Data.Configurations
{
    using BCKFreightTMS.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> person)
        {
            person
                .HasOne(p => p.Comunicators)
                .WithOne(com => com.Person)
                .HasForeignKey<Person>(p => p.ComunicatorsId);

            person
                .HasMany(p => p.ContactOrdersTo)
                .WithOne(ot => ot.Contact)
                .HasForeignKey(ot => ot.ContactId);
        }
    }
}
