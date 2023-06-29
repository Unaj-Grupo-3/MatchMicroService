using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class DateRequest2
    {
        public int MatchId { get; set; }
        public string Location { get; set; } // Grabar en formato Latitud;Longitud --> Ej: 34,7589635 ; 58,1452698
        public string Description { get; set; }
        public int ProposedUserId { get; set; }
        public int State { get; set; } // 3 Estados, 0 -> esperando confirmacion; 1 -> aceptado; -1 -> rechazado.
    }
}
