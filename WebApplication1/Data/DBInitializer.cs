using FaturaTakip.Data.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Type = FaturaTakip.Data.Models.Type;

namespace FaturaTakip.Data
{
    public class DBInitializer
    {
        public static void Initialize(InvoiceTrackContext context)
        {
            context.Database.EnsureCreated();

            if (context.Apartments.Any())
            {
                return;
            }

            var apartments = new Apartment[]
            {
                new Apartment{Block = Block.A, DoorNumber = 7, Floor = 2, Type = Type.TwoPlusOne},
                new Apartment{Block = Block.B, DoorNumber = 9, Floor = 2, Type = Type.TwoPlusOne}
            };

            context.Apartments.AddRange(apartments);
            context.SaveChanges();


            var debts = new Debt[]
            {
                new Debt { Bill = 250, Dues = 150, FKApartmentId = 1},
                new Debt { Bill = 450, Dues = 160, FKApartmentId = 2}
            };

            context.Debts.AddRange(debts);
            context.SaveChanges();

            var landLords = new Landlord[]
            {
                new Landlord
                {
                    Name = "Ahmet", LastName = "Yılmaz", Phone = "5554442265", Email = "ahmet.yilmaz@test.com",
                    GovermentId = "42565315684"
                },
                new Landlord
                {
                    Name = "Ali", LastName = "Yıldız", Phone = "5556663334", Email = "ali.yildiz@test.com",
                    GovermentId = "15352103642"
                }
            };

            context.Landlords.AddRange(landLords);
            context.SaveChanges();

            var rentedApartments = new RentedApartment[]
            {
                new RentedApartment { FKTenantId = 1,FKApartmentId = 1,FKLandlordId = 1, Status = true },
                new RentedApartment { FKTenantId = 2,FKApartmentId = 2,FKLandlordId = 2, Status = true }
            };

            context.RentedApartments.AddRange(rentedApartments);
            context.SaveChanges();

            var tenants = new Tenant[]
            {
                new Tenant
                {
                    Name = "Muhammet", LastName = "Kaya", Email = "muhammet.kaya@test.com", GovermentId = "30015652142",
                    PhoneNumber = "5456325561", LisencePlate = "54KF512"
                }
            };

        }
    }
}
