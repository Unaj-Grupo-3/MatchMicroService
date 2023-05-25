
namespace Application.Models
{
    public class UserMatchesFullResponse
    {
        public UserResponse UserMe { get; set; }
        public IList<UserMatchResp2> Matches { get; set; }
    }
}
