using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;
using GariunaiCloud_ToolSharing.PresentationLayer.DataTransferObjects;
using GariunaiCloud_ToolSharing.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GariunaiCloud_ToolSharing.PresentationLayer
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;

        public UsersController(IMapper mapper, IUserService userService, IJwtTokenService jwtTokenService)
        {
            _mapper = mapper;
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var listings = await _userService.GetUsersAsync();
            var dto = _mapper.Map<IList<UserInfo>>(listings);
            return Ok(dto);
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<UserInfo>(user);
            return Ok(dto);
        }

        /// <summary>
        /// Update your account information
        /// </summary>
        /// <param name="userInfo">
        /// Json of fields to update. Username change is not allowed.
        /// </param>
        /// <remarks>Requres auth</remarks>
        [HttpPost("updateInfo")]
        [Authorize]
        public async Task<IActionResult> UpdateInfo(UserInfo userInfo)
        {
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            if (userInfo.UserName != null)
            {
                return BadRequest("Username change is not supported");
            }

            var user = _mapper.Map<User>(userInfo);
            var result = await _userService.UpdateUserInfoAsync(userName, user);
            var dto = _mapper.Map<UserInfo>(result);

            return Ok(dto);
        }

        /// <summary>
        ///  Register a new user
        /// </summary>
        /// <param name="credentials">Username, emil and password</param>
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(UserCredentials credentials)
        {
            if (!Regex.IsMatch(credentials.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                return BadRequest("Invalid email");
            }

            if (!_userService.VerifyNewEmailAsync(credentials.Email).Result)
            {
                return BadRequest("Error email already exists");
            }

            var user = await _userService.RegisterUserAsync(credentials.Username, credentials.Email,
                credentials.Password);

            if (user == null)
            {
                return BadRequest("User already exists");
            }

            var dto = _mapper.Map<UserInfo>(user);
            return Ok(dto);
        }

        /// <summary>
        /// Login, returns JWT token
        /// </summary>
        /// <param name="credentials">Username ans password</param>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserCredentials credentials)
        {
            var user = await _userService.GetByCredentials(credentials.Username, credentials.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(_jwtTokenService.GenerateJwtToken(user));
        }
    }
}