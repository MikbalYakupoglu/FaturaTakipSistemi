using FaturaTakip.Data.Models;

namespace FaturaTakip.ViewModels
{
    public class ApartmentVM
    {
        public int Id { get; set; }
        public string LandlordName { get; set; }
        public Block Block { get; set; }
        public short Floor { get; set; }
        public int DoorNumber { get; set; }
        public Data.Models.Type Type { get; set; }
        public int RentPrice { get; set; }
        public bool Rented { get; set; }

    }
}
