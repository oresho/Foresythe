using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DTOs;
using Entities.Models;
using Foresythe.ActionFilters;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foresythe.Controllers
{
    [Route("api/v1/Users")]
    public class UserController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public UserController(IRepositoryManager repositoryManager,
            ILoggerService logger,
            IMapper mapper, UserManager<User> userManager, 
            SignInManager<User> signInManager)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{UserId}")]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        public IActionResult GetUser(string UserId)
        {
            var user = HttpContext.Items["user"] as User;

            return Ok(user);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddUser([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            var user = _mapper.Map<User>(userForRegistrationDto);
            await _userManager.CreateAsync(user, userForRegistrationDto.Password);
            await _repositoryManager.SaveAsync();


            return CreatedAtAction("GetUser", new { UserId = user.Id}, user);
        } 
        
        [HttpPost("signin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> SignIn([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                await _signInManager.PasswordSignInAsync
                    (userLoginDto.Username, userLoginDto.Password, true, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest(e);
            }

            return Ok();
        } 
        
        [HttpGet("signout")]
        public async Task<IActionResult> SignOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest(e);
            }

            return RedirectToRoute("api/v1/Home/Index");
        }

        [HttpDelete("{UserId}")]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            var user = HttpContext.Items["user"] as User;
            await _userManager.DeleteAsync(user);
            await _repositoryManager.SaveAsync();

            return NoContent();
        }
    }
}