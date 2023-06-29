using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserResponseAPI
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Description { get; set; }
        public LocationResponse Location { get; set; }
        public IList<ImageResponse> Images { get; set; }
        public GenderResponse Gender { get; set; }
    }
}
