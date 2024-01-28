namespace MusicPlayer.Models
{
    public readonly struct Search {
        public Search(string query, SortCriteria sortBy, bool order) {
            Query = query;
            SortBy = sortBy;
            Order = order;
        }

        public string Query { get; }
        public SortCriteria SortBy { get; }
        public bool Order{ get; } //true = asc, false = desc
    }
}