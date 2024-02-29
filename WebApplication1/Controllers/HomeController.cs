using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private IDistributedCache _distributedCache;
        public HomeController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboardData = new DashboardData
                {
                    TotalCustomerCount = 123,
                    TotalRevenue = 66,
                    TopSellingCountryName = "India",
                    TopSellingProductname = "Iron box"

                };
                string cacheKey = "MyCacheKey";
                string cacheKeyForTime = "CacheWithTimeLimit";
                var dtoBytes = await _distributedCache.GetAsync(cacheKey);
                var cacheTime = await _distributedCache.GetAsync(cacheKeyForTime);
                if(cacheTime != null)
                {
                    var cachedBytesTime = await _distributedCache.GetAsync(cacheKeyForTime);
                    ViewBag.cacheTime = cachedBytesTime;
                    
                }
                else
                {
                    string value = "This data will last log for 5 min";
                    var cacheOption = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    };
                    await _distributedCache.SetStringAsync(cacheKeyForTime, value,cacheOption);
                    
                }
                if (dtoBytes != null)
                {
                    var dtoJson = Encoding.UTF8.GetString(dtoBytes);
                    ViewBag.RedisCache = dashboardData;
                    ViewBag.RedisCacheResponse = dtoBytes ;
                    
                    //return JsonContent.DeserializeObject<DashboardData>(dtoJson);
                }
                else
                {
                    var serializedDto = JsonConvert.SerializeObject(dashboardData);
                    await _distributedCache.SetStringAsync(cacheKey, serializedDto);

                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return View();

        }
    }
}
