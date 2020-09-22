using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using CodeDemo.API.AppDatabase.Entities;

[assembly: InternalsVisibleTo("CodeDemo.API_TEST")]
namespace CodeDemo.API.AppDatabase.DatabaseContext
{
    /// <summary>
    /// Sqlight database
    /// </summary>
    public class PaymentDataContext : DbContext
    {
        /// <summary>
        /// If database file is not existed, it creates automatically.
        /// </summary>
        public PaymentDataContext() 
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Please find the database file from the location below
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString: "Filename=./AppDatabase/PaymentGateway.db");
        }

        /// <summary>
        /// To save all the payment process
        /// </summary> 
        public DbSet<PaymentProcess> PaymentProcess { get; set; }

        /// <summary>
        /// Login users for authentication.
        /// </summary>
        public DbSet<LoginUsers> LoginUsers { get; set; }
    }
}
