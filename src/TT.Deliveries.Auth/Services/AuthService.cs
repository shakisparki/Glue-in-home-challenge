using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Deliveries.Auth.Models;
using TT.Deliveries.Auth.Services.Contracts;
using TT.Deliveries.Core.Enums;

namespace TT.Deliveries.Auth.Services
{
    public class AuthService : IAuthService
    {
        public ServicePrincipal Authenticate(string username, string pwd)
            => TestUsers.SingleOrDefault(x => x.Username == username && x.Password == pwd);

        #region TestData
        private readonly List<ServicePrincipal> TestUsers = new()
        {
            new ServicePrincipal { Id = 1, Username = "admin", Password = "admin", Claims = new List<string> { Claims.Admin } },
            new ServicePrincipal { Id = 2, Username = "customer", Password = "customer", Claims = new List<string> { Claims.Customer } },
            new ServicePrincipal { Id = 2, Username = "partner", Password = "partner", Claims = new List<string> { Claims.Partner } }
        };
        #endregion
    }
}
