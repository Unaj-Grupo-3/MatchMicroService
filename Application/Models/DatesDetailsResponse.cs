using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class DatesDetailsResponse
    {
        public int DateId { get; set; }
        public DateTime Time { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public int DistanceBetweenUsers { get; set; } // Distancia existente entre usuarios
        public int DistanceBetweenUserDate1 { get; set; } // Distancia entre la localizacion de la cita y el usuario 1
        public int DistanceBetweenUserDate2 { get; set; } // Distancia entre la localizacion de la cita y el usuario 2
        public UserResponse2 User1 { get; set; } // Usuario creador de citas
        public UserResponse2 User2 { get; set; } // Usuario receptor de citas

        public int State { get; set; } // 3 Estados, 0 -> esperando confirmacion; 1 -> aceptado; 2 -> rechazado.
    }
}
