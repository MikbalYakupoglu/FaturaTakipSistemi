using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaturaTakip.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class InvoiceTrackContext : DbContext
    {
        public InvoiceTrackContext(DbContextOptions<InvoiceTrackContext> options) : base(options)
        {
            
        }

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<Landlord> Landlords{ get; set; }
        public DbSet<Message> Messages{ get; set; }
        public DbSet<Payment> Payments {get; set; }
        public DbSet<RentedApartment> RentedApartments { get; set; }
        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apartment>().ToTable("Apartment");
            modelBuilder.Entity<Debt>().ToTable("Debt");
            modelBuilder.Entity<Landlord>().ToTable("Landlord");
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<Payment>().ToTable("Payment");
            modelBuilder.Entity<RentedApartment>().ToTable("RentedApartment");
            modelBuilder.Entity<Tenant>().ToTable("Tenant");

        }
    }
}
