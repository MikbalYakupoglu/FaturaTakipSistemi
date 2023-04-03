namespace FaturaTakip.ViewModels
{
    public class RentedApartmentVM
    {
        public int Id { get; set; }
        public string ApartmentInfo { get; set; }
        public string TenantName { get; set; }
        public bool Status { get; set; }
        public DateTime RentTime { get; set; }
    }
}
