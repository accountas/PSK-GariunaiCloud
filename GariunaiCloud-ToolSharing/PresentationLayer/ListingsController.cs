using System.Security.Claims;
using AutoMapper;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;
using GariunaiCloud_ToolSharing.PresentationLayer.DataTransferObjects;
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
            catch (KeyNotFoundException)
            {
                return BadRequest("No such user");
            }
        }

        /// <summary>
        /// Get listings unavailable dates for a given date range
        /// </summary>
        /// <param name="id">Listing id</param>
        /// <param name="startDate">Start of the interest region</param>
        /// <param name="endDate">End of interest region</param>
        [HttpGet("{id:long}/availability")]
        public async Task<IActionResult> GetUnavailableDates(long id, [FromQuery] string startDate,
            [FromQuery] string endDate)
        {
            DateTime startDateTime, endDateTime;
            if (!_listingService.ListingExistsAsync(id).Result)
            {
                return NotFound();
            }

            try
            {
                startDateTime = DateTime.Parse(startDate);
                endDateTime = DateTime.Parse(endDate);
            }
            catch (Exception)
            {
                return BadRequest("Invalid date format");
            }

            if (startDateTime > endDateTime)
            {
                return BadRequest("Start date must be before end date");
            }

            var unavailableDates = await _listingService.GetUnavailableDatesAsync(id, startDateTime, endDateTime);
            var dates = unavailableDates.Select(d => d.ToString("yyyy-MM-dd")).ToList();
            return Ok(dates);
        }

        /// <summary>
        /// Edit an existing listing
        /// </summary>
        /// <param name="listingInfo">Listing information</param>
        /// <param name="id">Listing id</param>
        /// <remarks>Requires authentication</remarks>
        /// <returns>Updated listing object without Owner object</returns>
        [HttpPut("{id:long}")]
        [Authorize]
        public async Task<IActionResult> UpdateListing(ListingInfo listingInfo, long id)
        {
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var listing = _mapper.Map<Listing>(listingInfo);
            if(!_listingService.ListingExistsAsync(id).Result)
            {
                return NotFound();
            }
            if (id != listingInfo.ListingId)
            {
                return BadRequest("Cant change listing id");
            }
            if (!await _listingService.IsByUser(id, userName))
                return Unauthorized();

            var newListing = await _listingService.UpdateListingInfoAsync(listing);
            var dto = _mapper.Map<ListingInfo>(newListing);
            return Ok(dto);
        }

        /// <summary>
        /// Delete and existing listing
        /// </summary>
        /// <param name="id">listingID</param>
        /// <remarks>Requires authentication</remarks>
        /// <returns></returns>
        [HttpDelete("{id:long}")]
        [Authorize]
        public async Task<IActionResult> DeleteListing(long id)
        {
            var userName = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            if (await _listingService.GetListingAsync(id) == null)
                return NotFound();
            if (!await _listingService.IsByUser(id, userName))
                return Unauthorized();

            await _listingService.DeleteListingAsync(id);
            return Ok();
        }
        /// <summary>
        /// Search for listings that match some string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Return listings that contain the searchString in their titles</returns>
        [HttpGet()]
        public async Task<IActionResult> SearchListings([FromQuery]string searchString)
        {
            var listings = await _listingService.SearchListingsAsync(searchString);
            var payload = _mapper.Map<IList<ListingInfo>>(listings);
            return Ok(payload);
        }
    }
}