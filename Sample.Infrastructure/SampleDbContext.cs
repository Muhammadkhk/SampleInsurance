using Sample.Domain.PersonDetail;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Sample.Core.Infrastructure
{
    public class SampleDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        public DbSet<PersonDetail> PersonDetails { get; set; }

        public SampleDbContext(DbContextOptions<SampleDbContext> options, ILoggerFactory loggerFactory) : base(options)
            => _loggerFactory = loggerFactory;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonDetailEntityTypeConfiguration());
        }
        public class PersonDetailEntityTypeConfiguration : IEntityTypeConfiguration<Domain.PersonDetail.PersonDetail>
        {
            public void Configure(EntityTypeBuilder<Domain.PersonDetail.PersonDetail> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(p => p.Id).HasConversion(PersonDetailId => PersonDetailId.Value, dbId => new PersonDetailId(dbId));


                builder.OwnsOne(x => x.LastName)
                    .Property(x => x.Value)
                    .HasColumnName("LastName");

                builder.OwnsOne(x => x.City)
                    .Property(x => x.Value)
                    .HasColumnName("City");

                builder.OwnsOne(x => x.Country)
                    .Property(x => x.Value)
                    .HasColumnName("Country");
                
                builder.OwnsOne(x => x.InsurancePremiumDental)
                    .Property(x => x.Value)
                    .HasColumnName("InsurancePremiumDental");

                builder.OwnsOne(x => x.InsurancePremiumHospitalization)
                    .Property(x => x.Value)
                    .HasColumnName("InsurancePremiumHospitalization");

                builder.OwnsOne(x => x.InsurancePremiumOpration)
                    .Property(x => x.Value)
                    .HasColumnName("InsurancePremiumOpration");

                builder.OwnsOne(x => x.FirstName)
                    .Property(x => x.Value)
                    .HasColumnName("FirstName");

            }
        }

    }
    public static class AppBuilderDatabaseExtensions
    {
        public static void EnsureDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
                context.Database.Migrate();
            }
        }
    }
}