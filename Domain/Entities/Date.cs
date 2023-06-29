

namespace Domain.Entities
{
    public class Date
    {
        public int DateId { get; set; }
        public int MatchId { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public int State { get; set; } // 3 Estados, 0 -> esperando confirmacion; 1 -> aceptado; -1 -> rechazado.
        public int ProposedUserId { get; set; }

        public Match Match { get; set; }
    }
}
