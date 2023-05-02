using System;
using System.Collections.Generic;

namespace TT.Deliveries.Auth.Models
{
    public class ServicePrincipal
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public ICollection<string> Claims { get; set; }
    }
}
