using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaturaTakip.Models
{
    public class RentedApartmentDTO
    {
        public int Id { get; set; }
        public bool Status { get; set; }

        public TenantDTO Tenant { get; set; }
        public ApartmentDTO Apartment { get; set; }
        public LandlordDTO Landlord { get; set; }
    }
}
