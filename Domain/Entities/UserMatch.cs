
namespace Domain.Entities
{
    public class UserMatch
    {
        public int UserMatchId { get; set; }
        public int UserMainId { get; set; }
        public int UserSecundaryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Like { get; set; }

    }
}
