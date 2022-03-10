using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.DTOs
{
    public class SignedInUserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}