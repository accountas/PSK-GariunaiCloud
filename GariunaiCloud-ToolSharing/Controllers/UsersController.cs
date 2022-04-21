using AutoMapper;
using GariunaiCloud_ToolSharing.Controllers.DataTransferObjects;
using GariunaiCloud_ToolSharing.Models;
using GariunaiCloud_ToolSharing.Services;
using Microsoft.AspNetCore.Mvc;

namespace GariunaiCloud_ToolSharing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListings()
        {
            var listings = await _userService.GetUsersAsync();
            var dto = _mapper.Map<IList<UserPayload>>(listings);
            return Ok(dto);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetListing(string username)
        {
            var listing = await _userService.GetUserByUsernameAsync(username);
            if (listing == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<UserPayload>(listing);
            return Ok(dto);
        }
    }
}
