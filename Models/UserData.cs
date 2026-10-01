namespace MovieGallery.Models
{
    public class UserData
    {
        public List<UserData> list { get; set; } = new List<UserData>();
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
    }
}
