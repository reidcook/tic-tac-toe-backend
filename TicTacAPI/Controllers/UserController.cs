using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public class UserWins
        {
            public string name { get; set; }
            public int wins { get; set; }
            public UserWins(string name, int wins)
            {
                this.name = name;
                this.wins = wins;
            }
        }

        [HttpGet("leaderboard")]
        [Authorize]
        public async Task<List<UserWins>> leaderboard()
        {
            List<ApplicationUser> myUsers = await userManager.Users.ToListAsync();
            List<UserWins> winsList = new List<UserWins>();
            for(var i = 0; i < myUsers.Count(); i++)
            {
                int wins = (int)(myUsers[i].Wins == null ? 0 : myUsers[i].Wins);
                winsList.Add(new UserWins(myUsers[i].UserName, wins));
            }
            return winsList;
        }


        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);
    }
}
