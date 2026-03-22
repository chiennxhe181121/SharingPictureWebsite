using SharingPictureWebsite.Models;

namespace SharingPictureWebsite.ViewModels
{
    public class ModeratorDashboardViewModel
    {
        public PaginationViewModel<Picture> Pictures { get; set; } = new();
        public ModeratorStatusStatsViewModel Stats { get; set; } = new();
        public string CurrentFilter { get; set; } = "all";
    }
}
