using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudyFlow.DTOs;
using StudyFlow.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudyFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AuthUser> _userManager; 
        private readonly SignInManager<AuthUser> _signInManager; 
        private readonly IConfiguration _configuration; 
        private readonly ILogger<AuthController> _logger;
        public AuthController(UserManager<AuthUser> userManager, SignInManager<AuthUser> signInManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger; 
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var user = new AuthUser // Creates New User Object
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
            }; 

            var result = await _userManager.CreateAsync(user, registerDto.Password); // Place New User In DB

            if (result.Succeeded)
            {   // User successfully created
                _logger.LogInformation("[AuthAPIController] User Registered for {@username}", registerDto.Username);
                return Ok(new { Message = "User Regiostered Successfully"}); 
            }
            // User Creation Failed :´-(
            _logger.LogWarning("[AuthAPIController] user registration failed for {@username}", registerDto.Username); 
                return BadRequest(result.Errors); 
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            
            var user = await _userManager.FindByNameAsync(loginDto.Username); // Find User In User DB 

            if (user != null && await _userManager.CheckPasswordAsync(user,loginDto.Password)) 
            { // Success case
                _logger.LogInformation("[AuthAPICOntroller] user authorized for {@username}", loginDto.Username);
                var token = GenerateJwtToken(user); 
                return Ok(new {Token = token });
            }
            // Unauthorized attempt 

            _logger.LogWarning("[AuthAPIController] user not authorized for {@username}", loginDto.Username); 

            return Unauthorized(); 
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        { // Token Invalidation // Logout Protocol

        await _signInManager.SignOutAsync(); 
        _logger.LogInformation("[AuthAPIController] user logged out"); 

        return Ok(new {Messagew = "Logout Successful"}); 
            

        }


        private string GenerateJwtToken(AuthUser user)
        {
            /*=================
            // 1. Security Setup
            ================
            */

            var jwtKey =_configuration["Jwt:Key"]; //Secret key used for signature
            if (string.IsNullOrEmpty(jwtKey)) // Ensure Key is NOT null
            {
                _logger.LogError("[AuthAPIController] JWT key is missing from configuration"); 
                throw new InvalidOperationException("JWT Key is MISSING from configuration"); 
            } 

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)); // Reading Key from Configuration

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //Using HMAC SHA256 algorithm for substantial security 

            /*
            ===============================================
            2. Claims Creation

            
            pieces of information about
            the user. These include the username, email, and unique user ID.
            =======================================================
            */
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!), //Subject of token
                new Claim(JwtRegisteredClaimNames.Email, user.Email!), //Users Email                
                new Claim(ClaimTypes.NameIdentifier, user.Id), //Unique Identifier
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //Unique Identifier for token
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()) //Issued At Timestamp
                
            
            }; 


            /* 
            ==========================================
            
            3. Token Creation 

            Uses security credentials created in step 1 to sign JWT Token 

            ==========================================

*/
            var token = new JwtSecurityToken(
                issuer:_configuration["Jwt:Issuer"],
                audience:_configuration["Jwt:Audience"],
                claims:claims, 
                expires: DateTime.Now.AddMinutes(120), // Token Expiration rate in Minutes
                signingCredentials: credentials //Signs token with specified credentials 
            ); 


            _logger.LogInformation("[AuthAPiController] JWT Token created for {@username}", user.UserName); 

            return new JwtSecurityTokenHandler().WriteToken(token); 

}

    }




}