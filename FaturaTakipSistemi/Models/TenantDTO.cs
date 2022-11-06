using System.ComponentModel.DataAnnotations;
using FaturaTakip.Data.Models.Abstract;

namespace FaturaTakip.Models
{
    public class TenantDTO : User
    {
        [StringLength(9)]
        public string LisencePlate { get; set; }

    }
}
