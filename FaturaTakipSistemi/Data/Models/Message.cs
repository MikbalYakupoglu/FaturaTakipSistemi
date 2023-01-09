using FaturaTakip.Core.DataAccess;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaTakip.Data.Models
{
    public class Message : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int FKTenantId { get; set; }
        public int FKLandlordId { get; set; }

        [ForeignKey(nameof(FKTenantId))]
        public Tenant Tenant { get; set; }

        [ForeignKey(nameof(FKLandlordId))]
        public Landlord Landlord { get; set; }
    }
}
