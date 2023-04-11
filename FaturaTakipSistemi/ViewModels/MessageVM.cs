namespace FaturaTakip.ViewModels
{
    public class MessageVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string? TenantFullName { get; set; }
        public string? LandlordFullName { get; set; }
        public string? SenderName { get; set; }
    }
}
