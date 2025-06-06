﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using budget_api.Models;
using budget_api.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace budget_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // Register a new user (using AppUser directly)
        [HttpGet("email/{email}")]
        public async Task<ActionResult<AppUser>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Console.WriteLine($"UserName: {model.UserName}");

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                BirthDate = model.BirthDate,
                Salary = model.Salary ?? 1000,
                Balance = model.Balance ?? 0,
                SavingsBalance = model.SavingsBalance ?? 0
            };


            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION DURING REGISTRATION: " + ex.Message);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }




        // Login (Authentication)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                    return BadRequest("Invalid login request.");

                var foundUser = await _userManager.FindByEmailAsync(model.Email);
                if (foundUser == null)
                    return Unauthorized("Invalid username or password.");

                var result = await _signInManager.CheckPasswordSignInAsync(foundUser, model.Password, false);
                if (!result.Succeeded)
                    return Unauthorized("Invalid username or password.");

                var token = GenerateJwtToken(foundUser);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Login");
                return StatusCode(500, "Internal server error");
            }
        }


        // Get user profile (Return full AppUser)
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);  
        }

        // JWT Token Generator
        private string GenerateJwtToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
