
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FaturaTakip.Data.Models;
using Type = System.Type;

namespace FaturaTakip.Models
{
    public class ApartmentDTO
    {
        public int Id { get; set; }
        public short Floor { get; set; }
        public int DoorNumber { get; set; }
        public Type Type { get; set; }
        public Block Block { get; set; }
    }
}
