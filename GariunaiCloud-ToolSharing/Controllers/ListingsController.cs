using AutoMapper;
using GariunaiCloud_ToolSharing.Controllers.DataTransferObjects;
using GariunaiCloud_ToolSharing.Models;
using GariunaiCloud_ToolSharing.Services;
using Microsoft.AspNetCore.Mvc;

namespace GariunaiCloud_ToolSharing.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetListings()
        {
            var listings = await _listingService.GetListingsAsync();
            var payload = _mapper.Map<IList<ListingPayload>>(listings);
            return Ok(payload);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetListing(long id)
        {
            var listing = await _listingService.GetListingAsync(id);
            if (listing == null)
            {
                return NotFound();
            }

            var payload = _mapper.Map<ListingPayload>(listing);
            return Ok(payload);
        }

        [HttpPost]
        public async Task<IActionResult> CreateListing(ListingPayload listingPayload)
        {
            var listing = _mapper.Map<Listing>(listingPayload);
            try
            {
                var id = await _listingService.CreateListingAsync(listing, listingPayload.OwnerUsername);
                return Ok(id);
            }
            catch(KeyNotFoundException e)
            {
                return BadRequest("No such user");
            }
        }
    }
}
