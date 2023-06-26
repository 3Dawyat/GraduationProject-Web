using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Pharmacy_API.Controllers
{
    [Route("/[controller]/[action]")]
    [ApiController]
    public class FilterItemsController : ControllerBase
    {
        private readonly IService<VwItemsWithUnits> _ItemWithUnits;
        private readonly IWebHostEnvironment _webHost;

        public FilterItemsController(IWebHostEnvironment webHost, IService<VwItemsWithUnits> itemWithUnits)
        {
            _webHost = webHost;
            _ItemWithUnits = itemWithUnits;
        }

        //[HttpGet]
        //public async Task<IActionResult> FilterItems()
        //{

        //    var items = await _ItemWithUnits.GetAll();
        //    foreach (var item in items)
        //    {
        //        var filePath = Path.Combine(_webHost.WebRootPath, "drugs", item.ImageName);
        //        var jsonPath = Path.Combine(_webHost.WebRootPath, "JSON", "IdsImage.json");
        //        var fileExists = System.IO.File.Exists(filePath);
        //        if (!fileExists)
        //        {
        //         await System.IO.File.AppendAllTextAsync(jsonPath, $"{item.ItemId},");
        //        }
        //    }
        //    return Ok();
        //}
    }
}
