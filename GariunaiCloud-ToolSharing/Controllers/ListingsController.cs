using System.Security.Claims;
using AutoMapper;
using GariunaiCloud_ToolSharing.Controllers.DataTransferObjects;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GariunaiCloud_ToolSharing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly IListingService _listingService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ListingsController(IListingService listingService, IMapper mapper, IImageService imageService)
        {
            _listingService = listingService;
            _mapper = mapper;
            _imageService = imageService;
        }

        /// <summary>
        /// Get all listings
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetListings([FromQuery] ListingsFilter filters)
        {
            var listings = await _listingService.GetListingsAsync(filters);
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
            var userName = User.GetUsername();
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
        /// <param name="force">Force update in case of conflict</param>
        /// <remarks>Requires authentication</remarks>
        /// <returns>Updated listing object</returns>
        [HttpPut("{id:long}")]
        [Authorize]
        public async Task<IActionResult> UpdateListing(NewListingPayload listingInfo, long id)
        {
            var userName = User.GetUsername();
            
            var listing = _mapper.Map<Listing>(listingInfo);
            listing.ListingId = id;

            if (listingInfo.ETag == null)
            {
                return BadRequest("Missing etag");
            }
            if(!_listingService.ListingExistsAsync(id).Result)
            {
                return NotFound();
            }

            if (!await _listingService.IsByUser(id, userName))
            {
                return Unauthorized();
            }
                

            Listing? newListing;
            try
            {
                newListing = await _listingService.UpdateListingInfoAsync(listing);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }
            
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

        [HttpGet("{id:long}/image")]
        public async Task<IActionResult> GetListingImage(long id)
        {
            var listing = await _listingService.GetListingAsync(id);
            if (listing == null) 
            {
                return NotFound();
            }
            if(listing.Image == null)
            {
                return NotFound();
            }
            return File(listing.Image.ImageData, "image/jpeg");
        }
        [HttpPut("{id:long}/image")]
        [Authorize]
        public async Task<IActionResult> GetListingImage(long id, IFormFile file)
        {
            if(!_listingService.ListingExistsAsync(id).Result)
            {
                return NotFound();
            }
            if (!_listingService.IsByUser(id, User.GetUsername()).Result)
            {
                return Unauthorized();
            }

            DbImage image;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                image = await _imageService.UploadImageAsync(memoryStream.ToArray());
            }

            await _listingService.SetImageAsync(id, image);
            return Ok();
        }

    }
}