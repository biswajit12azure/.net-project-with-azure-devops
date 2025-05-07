using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Application.DTOs.Account
{
    public class AuthenticationRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
