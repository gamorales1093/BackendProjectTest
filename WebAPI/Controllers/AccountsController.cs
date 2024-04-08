using Application.Interfaces;
using Commons.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IJWTService _jwtService;

        public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJWTService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<ActionResult<UserToken>> Create([FromBody] UserInfo userRequest)
        {
            var user = new IdentityUser { UserName = userRequest.Email, Email = userRequest.Email };
            var result = await _userManager.CreateAsync(user, userRequest.Password);

            if (result.Succeeded)
            {
                _ = userRequest.IsAdmin
                    ? await _userManager.AddToRoleAsync(user, "Admin")
                    : await _userManager.AddToRoleAsync(user, "Employee");

                return Ok("User Created");
            }
            else
            {
                StringBuilder errors = new();
                foreach (var item in result.Errors)
                    errors.AppendLine($"{item.Description}");

                return BadRequest(errors.ToString());
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserLogin userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await this._jwtService.BuildToken(userInfo);
            }
            else
            {                
                return BadRequest("Invalid login attempt");
            }
        }
    }
}