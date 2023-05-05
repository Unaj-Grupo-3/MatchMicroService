namespace Domain.Entities
{
    public class UserMatch
    {
        public int UserMatchId { get; set; }
        public int User1 { get; set; }
        public int User2 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // 3 estados del Like. null => Todavia no lo vio.
        //                     true => Le dio Like (checkear el otro)
        //                     false => Le dio Dislike

        public bool LikeUser2 { get; set; }  
        public bool LikeUser1 { get; set; }
    }
}
