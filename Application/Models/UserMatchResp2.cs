
namespace Application.Models
{
    public class UserMatchResp2
    {
        public int UserMatchId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public UserResponse? userInfo { get; set; }
    }
}
