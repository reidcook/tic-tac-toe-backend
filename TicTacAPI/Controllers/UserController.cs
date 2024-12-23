using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TicTacAPI.Data;

namespace TicTacAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("getUser")]
        public async Task<ApplicationUser> GetUser()
        {
            var user = await GetCurrentUserAsync();
            return user;
        }

        [HttpPut("{email}")]
        public async Task<ApplicationUser> AddUsername(string email, string username)
        {
            var user = await userManager.FindByEmailAsync(email);
            user.UserName = username;
            await userManager.UpdateAsync(user);
            return user;
        }

        [HttpPost("addWin")]
        public async Task<ApplicationUser> addWin()
        {
            var user = await GetCurrentUserAsync();
            user.Wins += 1;
            await userManager.UpdateAsync(user);
            return user;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }


        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);
    }
}
