using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using FaturaTakip.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Data
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
            modelBuilder.Entity<Apartment>().ToTable("Apartments");
            modelBuilder.Entity<Debt>().ToTable("Debts");
            modelBuilder.Entity<Landlord>().ToTable("Landlords");
            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<Payment>().ToTable("Payments");
            modelBuilder.Entity<RentedApartment>().ToTable("RentedApartments");
            modelBuilder.Entity<Tenant>().ToTable("Tenants");
        }
    }
}

//modelBuilder.Entity<Apartment>().ToTable("Apartment")
//    .Property(a => a.Id)
//.ValueGeneratedOnAdd();

//modelBuilder.Entity<Debt>().ToTable("Debt")
//    .Property(d => d.Id)
//    .ValueGeneratedOnAdd();

//modelBuilder.Entity<Landlord>().ToTable("Landlord")
//    .Property(l => l.Id)
//    .ValueGeneratedOnAdd();

//modelBuilder.Entity<Message>().ToTable("Message")
//    .Property(m => m.Id)
//    .ValueGeneratedOnAdd();

//modelBuilder.Entity<Payment>().ToTable("Payment")
//    .Property(p => p.Id)
//    .ValueGeneratedOnAdd();

//modelBuilder.Entity<RentedApartment>().ToTable("RentedApartment")
//    .Property(r => r.Id)
//    .ValueGeneratedOnAdd();

//modelBuilder.Entity<Tenant>().ToTable("Tenant")
//    .Property(t => t.Id)
//    .ValueGeneratedOnAdd();
