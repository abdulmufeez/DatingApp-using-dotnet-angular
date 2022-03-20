using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{    
    public class UsersController : BaseController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // api/users
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        //api/user/2
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApplicationUser>> GetUser(int id) 
        {
            var userInDb = await _context.Users.FindAsync(id);
            return userInDb;
        }
    }
}