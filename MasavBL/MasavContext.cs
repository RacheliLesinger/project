using MasavBL.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MasavBL
{
    public class MasavContext : DbContext
    {
        public MasavContext() : base("name=MasavDBConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MasavContext, Migrations.Configuration>());
            
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<BroadcastHistory> BroadcastHistories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRates> CurrencyRates { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Paying> Payings { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<CodeBank> CodeBanks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Paying>()
                .HasRequired(e => e.Customers)
                .WithMany()
                .WillCascadeOnDelete(false);
         

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder
            //    .Entity<Paying>()
            //    .HasRequired(e => e.Customers)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Paying>()
            //    .HasKey(e => e.Id)
            //    .WillCascadeOnDelete(false);


        }


    }
}