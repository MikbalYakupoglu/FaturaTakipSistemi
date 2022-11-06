using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaTakip.Models
{
    public class DebtDTO
    {
        public int Id { get; set; }
        public int Dues { get; set; }
        public int Bill { get; set; }
        public ApartmentDTO Apartment { get; set; }
    }
}
