using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserResponse2
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? Images { get; set; }
    }
}
