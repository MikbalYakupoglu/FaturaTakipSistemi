using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaTakip.Models
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int Amount { get; set; }

        public TenantDTO Tenant { get; set; }
        public ApartmentDTO  Apartment { get; set; }
    }
}
