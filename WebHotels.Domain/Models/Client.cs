using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHotels.Domain.Models
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public char Gender { get; set; }
        public string Nationality { get; set; }
        public bool IsVIP { get; set; } = false;
    }
}
