using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaTakip.Data.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int FKTenantId { get; set; }
        public int FKApartmentId { get; set; }

        [ForeignKey(nameof(FKTenantId))]
        public Tenant Tenant { get; set; }
        
        [ForeignKey(nameof(FKApartmentId))]
        public Apartment Apartment { get; set; }
    }
}
