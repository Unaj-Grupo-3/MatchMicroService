
namespace Application.Models
{
    public class UserMatchResponse
    {
        public int User1 { get; set; }
        public int User2 { get; set; }
        public bool IsMatch { get; set; }

        public MatchResponse2 Match { get; set; }
    }
}
