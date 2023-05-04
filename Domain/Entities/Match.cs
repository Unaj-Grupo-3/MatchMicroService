
namespace Domain.Entities
{
    public class Match
    {
        public int MatchId { get; set; }
        public Guid User1Id { get; set; }
        public Guid User2Id { get; set;}
        public DateTime CreatedAt { get; set; }

        public Date Date { get; set; }
    }
}
