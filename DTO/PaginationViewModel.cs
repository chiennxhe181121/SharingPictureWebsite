namespace SharingPictureWebsite.ViewModels
{
    public class PaginationViewModel<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int TotalItems { get; set; }
        public int PageSize { get; set; } = 5;

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
