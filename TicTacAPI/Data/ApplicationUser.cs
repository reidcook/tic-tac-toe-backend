using Microsoft.AspNetCore.Identity;

namespace TicTacAPI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int? Wins { get; set; } = 0;
    }
}
