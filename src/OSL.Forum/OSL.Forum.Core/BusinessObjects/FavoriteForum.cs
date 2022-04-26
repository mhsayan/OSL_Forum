namespace OSL.Forum.Core.BusinessObjects
{
    public class FavoriteForum
    {
        public long Id { get; set; }
        public string ApplicationUserId { get; set; }
        public long ForumId { get; set; }
        public Forum Forum { get; set; }

    }
}
