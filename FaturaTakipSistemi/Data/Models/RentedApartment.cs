using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaturaTakip.Data.Models
{
    public class RentedApartment
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public int FKTenantId { get; set; }
        public int FKApartmentId { get; set; }
        public int FKLandlordId { get; set; }

        [ForeignKey(nameof(FKTenantId))]
        public Tenant Tenant { get; set; }

        [ForeignKey(nameof(FKApartmentId))]
        public Apartment Apartment { get; set; }

        [ForeignKey(nameof(FKLandlordId))]
        public Landlord Landlord { get; set; }
    }
}
