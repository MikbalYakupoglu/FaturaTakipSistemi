using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaTakip.Models
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public TenantDTO Tenant { get; set; }
        public LandlordDTO Landlord { get; set; }
    }
}
