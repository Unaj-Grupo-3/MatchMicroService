
namespace Application.Models
{
    public class UserMatchResp2
    {
        public int UserMatchId { get; set; }
        public int MatchId { get; set; }
        public int User1 { get; set; }
        public int User2 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool View1 { get; set; }
        public bool View2 { get; set; }

        public UserResponse? userInfo { get; set; }
    }
}
