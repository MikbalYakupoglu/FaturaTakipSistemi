using System.ComponentModel.DataAnnotations;

namespace FaturaTakip.Data.Models
{
    public class Landlord
    {
        public int Id { get; set; }
        
        [StringLength(50)]
        public string Name { get; set; }
        
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(11)]
        public string GovermentId { get; set; }
        
        [StringLength(100)]
        public string Email { get; set; }
        
        [StringLength(10)]
        public string Phone { get; set; }

        List<Apartment> Apartments { get; set; }
    }
}
