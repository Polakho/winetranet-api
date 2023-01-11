using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using winetranet_api.DTO;
using winetranet_api.Entities;

namespace winetranet_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly WinetranetContext _context;
        private readonly DbSet<User> _users;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthController(WinetranetContext context, IMapper mapper,
                              IConfiguration configuration)
        {
            _context = context;
            _users = _context.Users;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(Register register)
        {
            if (_users.Any(x => x.Email == register.Email))
            {
                return BadRequest("User already exists with this email.");
            }

            var user = new User();

            user.Firstname = register.Firstname;
            user.Lastname = register.Lastname;
            user.Email = register.Email;
            user.Phone = register.Phone;
            user.PhoneMobile = register.PhoneMobile;
            user.Service = register.Service;
            user.Site = register.Site;
            user.Role = "VISITOR";

            CreatePasswordHash(register.Password, out string passwordHash, out string passwordSalt);

            bool isValid = VerifyPasswordHash(register.Password, passwordHash, passwordSalt);
            if (!isValid)
            {
                return BadRequest("Wrong password");
            }
            
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            _users.Add(user);
            await _context.SaveChangesAsync();

            UserDTO? User = _mapper.Map<UserDTO>(user);

            return Ok(User);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(Login login)
        {
            var user = _users.FirstOrDefault(x => x.Email == login.Email);

            if (user == null)
            {
                return BadRequest("This email does not exist");
            }


            if (!VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password");
            }

            string token = CreateToken(user);

            return Ok(token);

        }

        private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            /*using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }*/
            passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
            passwordHash = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);

        }
        private bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
        {
           
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
            /*using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }*/
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>() {
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }
    }
}
