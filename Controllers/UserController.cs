using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using winetranet_api.DTO;
using winetranet_api.Entities;

namespace winetranet_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly WinetranetContext _context;
        private readonly DbSet<User> _users;
        private readonly IMapper _mapper;
        public UserController(WinetranetContext context, IMapper mapper)
        {
            _context = context;
            _users = _context.Users;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            return Ok(_users.Select(User => _mapper.Map<UserDTO>(User)).ToList().OrderBy(x => x.Id));
        }

        [Authorize]
        [HttpGet("GetUserById")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            UserDTO? User = _mapper.Map<UserDTO>(_users.FirstOrDefault(x => x.Id == id));

            return User == null ? NotFound() : Ok(User);
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult<UserDTO>> UpdateUser(UserDTO userToUpdate)
        {
            var user = await _users.FirstOrDefaultAsync(x => x.Id == userToUpdate.Id);

            if (user == null)
            {
                User userMapped = _mapper.Map<User>(userToUpdate);
                userMapped.Id = userToUpdate.Id;
                userMapped.Firstname = userToUpdate.Firstname;
                userMapped.Lastname = userToUpdate.Lastname;
                userMapped.Role = userToUpdate.Role;
                userMapped.Email = userToUpdate.Email;
                userMapped.Phone = userToUpdate.Phone;
                userMapped.PhoneMobile = userToUpdate.PhoneMobile;
                userMapped.Site = userToUpdate.Site;
                userMapped.Service = userToUpdate.Service;

                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<UserDTO>(userMapped));
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost("addUser")]
        public async Task<ActionResult<UserDTO>> CreateUser(UserDTO userToCreate)
        {
            var user = new User();

            if (userToCreate != null)
            {
                user.Id = userToCreate.Id;
                user.Firstname = userToCreate.Firstname;
                user.Lastname = userToCreate.Lastname;
                user.Role = userToCreate.Role;
                user.Email = userToCreate.Email;
                user.Phone = userToCreate.Phone;
                user.PhoneMobile = userToCreate.PhoneMobile;
                user.Site = userToCreate.Site;
                user.Service = userToCreate.Service;

                _users.Add(user);
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<UserDTO>(user));
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpDelete("deleteUser")]
        public async Task<ActionResult<UserDTO>> DeleteUser(int userId)
        {
            var user = await _users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user != null)
            {
                _users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
    } 
}
