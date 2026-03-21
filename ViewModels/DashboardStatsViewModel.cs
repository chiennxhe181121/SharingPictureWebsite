namespace SharingPictureWebsite.ViewModels
{
    public class DashboardStatsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalImages { get; set; }
        public int ActiveUsers { get; set; }
        public int BannedUsers { get; set; }
        public int ApprovedImages { get; set; }
        public int ActivePercent { get; set; }
        public int BannedPercent { get; set; }
    }
}
