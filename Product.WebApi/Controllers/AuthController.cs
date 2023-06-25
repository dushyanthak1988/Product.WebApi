using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Product.WebApi.Concrete;
using Product.WebApi.Data;
using Product.WebApi.Models.RequestModel;
using Product.WebApi.Models.ResponseModel;
using System.Security.Claims;

namespace Product.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;


        public AuthController(ApplicationDbContext ApplicationDbContext, ITokenService tokenService, UserManager<ApplicationUser> userManager)
        {
            this._ApplicationDbContext = ApplicationDbContext ?? throw new ArgumentNullException(nameof(ApplicationDbContext));
            this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this._userManager = userManager;
        }
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel is null)
            {
                return BadRequest("Invalid client request");
            }

            var userModel = await _userManager.FindByNameAsync(loginModel.UserName);

            if (userModel == null)
            {
                return BadRequest("Invalid UserName");
            }



            if (!await _userManager.CheckPasswordAsync(userModel, loginModel.Password))
            {
                return BadRequest("Invalid UserName and password");
            }

            AuthenticatedResponse user = new AuthenticatedResponse
            {
                UserName = loginModel.UserName,
            };


            //var user = _ApplicationDbContext.Users.FirstOrDefault(u =>
            //    (u.UserName == loginModel.UserName) && (u.PasswordHash == loginModel.Password));
            //if (user is null)
            //    return Unauthorized();
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginModel.UserName),
            new Claim(ClaimTypes.Role, "Manager")
        };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            _ApplicationDbContext.SaveChanges();
            return Ok(new AuthenticatedResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
    }
}

