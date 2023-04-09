using System.ComponentModel.DataAnnotations.Schema;
using FaturaTakip.Data.Models.Abstract;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FaturaTakip.Data.Models
{
    public class RentedApartment : IEntity
    {
        public RentedApartment()
        {
            RentTime = DateTime.Now;
        }

        public int Id { get; set; }
        public bool Status { get; set; }
        public DateTime RentTime { get; set; }

        public int FKTenantId { get; set; }
        public int FKApartmentId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(FKTenantId))]
        public Tenant Tenant { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(FKApartmentId))]
        public Apartment Apartment { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
