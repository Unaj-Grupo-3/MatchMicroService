using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class DateRequest
    {
        public int DateId { get; set; }
        public int MatchId { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public int State { get; set; } // 3 Estados, 0 -> esperando confirmacion; 1 -> aceptado; 2 -> rechazado.
    }
}
