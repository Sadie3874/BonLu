namespace BonLu.Models
{
    public class FriendsList
    {
        public int Id { get; set; }

        // UserId corresponds to the IdentityUser.Id who created this friendslist
        public string? UserId { get; set; }

        // Navigation property for the enrollments
        public ICollection<ApplicationUser> Friends { get; set; } = new List<ApplicationUser>();
    }
}
