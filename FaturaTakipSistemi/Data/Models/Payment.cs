using FaturaTakip.Data.Models.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaTakip.Data.Models
{
    public class Payment : IEntity
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string? Info { get; set; }
        public int FKTenantId { get; set; }
        public int FKRentedApartmentId { get; set; }

        [ForeignKey(nameof(FKTenantId))]
        public Tenant Tenant { get; set; }
        
        [ForeignKey(nameof(FKRentedApartmentId))]
        public RentedApartment RentedApartment { get; set; }
    }
}
