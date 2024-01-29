

namespace Application.Models
{
    public class DateEditRequest
    {
        public int DateId { get; set; }
        public int State { get; set; } // 3 Estados, 0 -> esperando confirmacion; 1 -> aceptado; -1 -> rechazado.
    }
}
