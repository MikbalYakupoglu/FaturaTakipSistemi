using FaturaTakip.Data.Models.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaTakip.Data.Models
{
    public class Debt : IEntity
    {
        public int Id { get; set; }
        public int Dues { get; set; }
        public int Bill { get; set; }
        public int FKApartmentId { get; set; }

        [ForeignKey(nameof(FKApartmentId))]
        public Apartment Apartment { get; set; }
    }
}
