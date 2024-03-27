using Microsoft.AspNetCore.Mvc;
using RTB.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace RTB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BiddingController : ControllerBase
    {
        private readonly ILogger<BiddingController> _logger;

        public BiddingController(ILogger<BiddingController> logger)
        {
            _logger = logger;
        }

        private readonly List<Advertisement> _advertisements = new List<Advertisement>
        {
            new Advertisement { Country = "france", Category = "food", Price = 3 },
            new Advertisement { Country = "france", Category = "food", Price = 4 },

            new Advertisement { Country = "france", Category = "tech", Price = 5 },

            new Advertisement { Country = "belgium", Category = "food", Price = 6 },
            new Advertisement { Country = "belgium", Category = "food", Price = 1 },
            new Advertisement { Country = "belgium", Category = "tech", Price = 3 },

            new Advertisement { Country = "USA", Category = "tech", Price = 2 },

            new Advertisement { Country = "USA", Category = "tech", Price = 5 },

            new Advertisement { Country = "USA", Category = "food", Price = 6 }
        };

        [HttpPost]
        [Route("ad")]
        public ActionResult<Advertisement> GetHighestPricedAd([FromBody] UserInfo userInfo)
        {
            _logger.LogInformation("Received POST request to /Bidding/ad");

            if (userInfo == null)
            {
                _logger.LogError("User information is missing in the request body.");
                return BadRequest("User information is missing in the request body.");
            }

            _logger.LogInformation("User information received: {@UserInfo}", userInfo);

            var matchingAds = GetMatchingAdvertisements(userInfo);
            if (matchingAds.Any())
            {
                var highestPricedAd = GetHighestPricedAdvertisement(matchingAds);
                _logger.LogInformation("Highest priced advertisement found: {@Advertisement}", highestPricedAd);
                return highestPricedAd;
            }

            _logger.LogInformation("No matching advertisements found.");
            return NotFound("No matching advertisements found.");
        }

        private List<Advertisement> GetMatchingAdvertisements(UserInfo user)
        {
            return _advertisements.Where(ad =>
                ad.Country.ToLower() == user.Country.ToLower() &&
                ad.Category.ToLower() == user.Category.ToLower()).ToList();
        }

        private Advertisement GetHighestPricedAdvertisement(List<Advertisement> advertisements)
        {
            return advertisements.OrderByDescending(ad => ad.Price).FirstOrDefault();
        }
    }
}
