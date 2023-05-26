using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentcarApp.Models;
using RentcarApp.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentcarApp.Controllers
{
    [ApiController]
    [Route("Userlist")]
    public class UserController : ControllerBase
    {
        private readonly RentcarContext _context;
        private readonly AuthService _authService;

        public UserController(RentcarContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get(string email )
        {
            try
            {
                var email2 = "51ozgurozer@gmail.com";
                var password = "123456"; // Kullanıcının şifresini burada belirtmeniz gerekmektedir

                var isValidCredentials = _authService.ValidateCredentials(email2, password);

                if (isValidCredentials)
                {
                    var users = await _context.Users.ToListAsync();
                    return Ok(users);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
