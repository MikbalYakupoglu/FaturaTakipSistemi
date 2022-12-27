using System.ComponentModel.DataAnnotations;
using FaturaTakip.Data.Models.Abstract;

namespace FaturaTakip.Data.Models
{
    public class Tenant : User
    {
        [StringLength(9)]
        public string? LisencePlate { get; set; }


    }
}
