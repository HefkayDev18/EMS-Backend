using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Models.ViewModels;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly DataProtectionTokenProviderOptions _tokenOptions;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, IUnitOfWork unitOfWork, IEmailService emailService, IOptions<DataProtectionTokenProviderOptions> tokenOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _tokenOptions = tokenOptions.Value;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _unitOfWork.Employees.GetEmployeeByEmailAsync(model.Email);

            var role = await _unitOfWork.Sys_Roles.GetRoleByNameAsync("User");

            if (employee == null)
            {
                return BadRequest("You are not registered as an employee. Please contact the admin.");
            }

            var existingUser = await _unitOfWork.Sys_Users.GetUserByEmail(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Account with this email already exists.");
                return BadRequest(ModelState);
            }

            var userName = GenerateUserName(model.Name);

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                var newUser = new SystemUsers
                {
                    IdentityId = user.Id,
                    FullName = model.Name,
                    Email = model.Email,
                    DateJoined = DateTime.Now,
                    EmployeeId = employee.EmployeeId,
                    RoleId = role.RoleId,
                    Employee = employee,
                    Role = role
                };

                await _unitOfWork.Sys_Users.AddUserAsync(newUser);
                await _unitOfWork.CompleteAsync();

                employee.UserId = newUser.UserId;
                employee.RoleId = newUser.RoleId;
                employee.IsActive = true;
                await _unitOfWork.Employees.UpdateEmployeeAsync(employee);

                await _unitOfWork.CompleteAsync();

                //return Ok(new { UserId = user.Id });
                return Ok("Account created successfully");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await _userManager.FindByEmailAsync(model.Email);

            if (appUser == null)
            {
                return Unauthorized("Invalid login attempt.");
            }

            var user = await _unitOfWork.Employees.GetEmployeeByEmailAsync(model.Email);

            if (user == null || user.IsActive == false)
            {
                return Unauthorized("Access revoked, you are not an active employee!");
            }

            var result = await _signInManager.PasswordSignInAsync(appUser.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(appUser);
                var generatedToken = GenerateJwtToken(appUser);

                var userData = new
                {
                    employeeObj = user,
                    email = user.Email,
                    name = user.FullName,
                    role = roles.FirstOrDefault()
                };

                return Ok(new 
                { 
                    token = generatedToken.Result,
                    user = userData
                });
            }

            return Unauthorized("Invalid login attempt.");
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            //Include a check later, to prevent persistent resetting of a user
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var expirationMinutes = _tokenOptions.TokenLifespan.TotalMinutes;

            //var resetLink = Url.Action("ResetPassword", "Auth", new { token, email = user.Email }, Request.Scheme);
            //var resetLink = $"http://localhost:5173/auth/resetpassword?token={token}&email={user.Email}";
            var resetLink = $"http://localhost:5173/auth/resetpassword?token={HttpUtility.UrlEncode(token)}&email={HttpUtility.UrlEncode(user.Email)}";
            var employeeObj = await _unitOfWork.Employees.GetEmployeeByEmailAsync(user.Email);

            var subject = "Password Reset";
            var message = $@"
                <html>
                <body style='font-family: 'Roboto', sans-serif; line-height: 1.5;'>
                    <p>Dear {employeeObj.FullName},</p>
                    <p>A request to reset your password was recieved. Please click the link below to reset your password. This link will expire in {expirationMinutes} minutes.</p>
                    <p><a href='{resetLink}' style='display: inline-block; padding: 8px 18px; color: #fff; background-color: #007bff; text-decoration: none; border-radius: 5px;'>Reset Password</a></p>
                    <p>If you did not initiate this request, please ignore this email. Your password will remain unchanged.</p>
                    <p>Best regards,<br>Employee Management System Team.</p>
                </body>
                </html>";

            await _emailService.SendEmailAsync(user.Email, subject, message);

            return Ok("Password reset link has been sent to your email.");
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Invalid request");
            }

            //var token = HttpUtility.UrlDecode(model.Token);
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return Ok("Password has been reset successfully.");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }




        #region private functions
        private string GenerateUserName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 4)
            {
                throw new ArgumentException("Full name must be at least 4 characters long.");
            }

            var firstTwo = fullName.Substring(0, 2).ToLower();
            var lastTwo = fullName.Substring(fullName.Length - 2).ToLower();
            //var random = Guid.NewGuid().ToString("N").Substring(0, 5);
            var random = new Random();
            var randomNumbers = random.Next(10000, 99999).ToString();
            var username = $"{firstTwo}{lastTwo}{randomNumbers}";

            return username;
        }

       

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);
            var roleClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role));
            claims.AddRange(roleClaims);

            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT_KEY is not set.");
            }

            //var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //private async Task<string> GenerateJwtToken(ApplicationUser user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id)
        //    };

        //    var userClaims = await _userManager.GetClaimsAsync(user);
        //    claims.AddRange(userClaims);

        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    var roleClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role));
        //    claims.AddRange(roleClaims);

        //    var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

        //    if (string.IsNullOrEmpty(jwtKey))
        //    {
        //        throw new InvalidOperationException("JWT_KEY is not set.");
        //    }

        //    var base64EncodedKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtKey));

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(base64EncodedKey));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(30),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


        //private async Task<string> GenerateJwtToken(ApplicationUser user)
        //{
        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id)
        //    };

        //    var userClaims = await _userManager.GetClaimsAsync(user);

        //    var userRoles = await _userManager.GetRolesAsync(user);

        //    var roleClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

        //    var allClaims = claims.Concat(userClaims).Concat(roleClaims).ToList();

        //    //var roleClaims = new List<Claim>();
        //    //foreach (var role in userRoles)
        //    //{
        //    //    roleClaims.Add(new Claim(ClaimTypes.Role, role));
        //    //}
        //    //var allClaims = claims.Concat(userClaims).Concat(roleClaims);

        //    var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

        //    if (string.IsNullOrEmpty(jwtKey))
        //    {
        //        throw new InvalidOperationException("JWT_KEY is not set.");
        //    }

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: allClaims,
        //        expires: DateTime.Now.AddMinutes(30),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        #endregion
    }
}

