using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentcarApp.Models;

namespace RentcarApp.Controllers
{
    [ApiController]
    [Route("Userlist")]
    public class UserController : ControllerBase
    {
        private readonly RentcarContext _context;

        public UserController(RentcarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }
    }
}
// llllll
