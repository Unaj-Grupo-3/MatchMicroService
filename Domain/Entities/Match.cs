
namespace Domain.Entities
{
    public class Match
    {
        public int MatchId { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set;}
        public DateTime CreatedAt { get; set; }

        public IList<Date> Date { get; set; }
    }
}
