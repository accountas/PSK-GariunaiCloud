using System.Security.Claims;
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
    public class ListingsController : ControllerBase
    {
        private readonly IListingService _listingService;
        private readonly IMapper _mapper;

        public ListingsController(IListingService listingService, IMapper mapper)
        {
            _listingService = listingService;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Get all listings
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetListings()
        {
            var listings = await _listingService.GetListingsAsync();
            var payload = _mapper.Map<IList<ListingInfo>>(listings);
            return Ok(payload);
        }
        
        /// <summary>
        /// Get listing by id
        /// </summary>
        /// <param name="id">Id of listing to return</param>
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetListing(long id)
        {
            var listing = await _listingService.GetListingAsync(id);
            if (listing == null)
            {
                return NotFound();
            }

            var payload = _mapper.Map<ListingInfo>(listing);
            return Ok(payload);
        }
        
        /// <summary>
        /// Create a new listing, returns new listing id
        /// </summary>
        /// <param name="listingInfo">Listings information</param>
        /// <remarks>Requires authentication, assigns listing to the user which is logged in</remarks>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateListing(NewListingPayload listingInfo)
        {
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var listing = _mapper.Map<Listing>(listingInfo);
            try
            {
                var id = await _listingService.CreateListingAsync(listing, userName);
                return Ok(id);
            }
            catch(KeyNotFoundException)
            {
                return BadRequest("No such user");
            }
        }
        /// <summary>
        /// Edit and existing listing
        /// </summary>
        /// <param name="listingInfo">Listing information</param>
        /// <remarks>Requires authentication</remarks>
        /// <returns>Updated listing object without Owner object</returns>
        [HttpPost("updateListing")]
        [Authorize]
        public async Task<IActionResult> UpdateListing(ListingInfo listingInfo)
        {
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var listing = _mapper.Map<Listing>(listingInfo);
            try
            {
                var newListing = await _listingService.UpdateListingInfoAsync(listing, userName);
                return Ok(newListing);
            }
            catch(KeyNotFoundException)
            {
                return BadRequest("No such user");
            }
        }
    }
}
