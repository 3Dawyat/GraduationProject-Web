
using Pharmacy_DomainModels.Models.DB_Models;
using System;

namespace Pharmacy_API.Controllers
{
    [Route("/[controller]/[action]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IService<VwItemsWithUnits> _item;
        private readonly IMapper _Mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ItemController(IService<VwItemsWithUnits> item, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _item = item;
            _Mapper = mapper;
            _hostEnvironment = hostEnvironment;
           
        }
        [HttpGet, Authorize]
        public async Task<IActionResult> GetAllItems()
        {
            var url = Url.Action("", "");
            var items = await _item.GetAll();
            var model = _Mapper.Map<List<ItemModel>>(items);

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            model.ForEach(
                item =>
                {
                    item.ImageName = $"{baseUrl}/drugs/{item.ImageName}";
                });
            return Ok(model);
        }
     
        [HttpGet("{id}:int"), Authorize]
        public async Task<IActionResult> GetItemDetails(int id)
        {
            var item = await _item.GetObjectBy(a => a.ItemUnitId == id);
            if (item == null)
                return BadRequest($"No Item With Id {id}");

            var model = _Mapper.Map<DetailsItemModel>(item);

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            model.ImageName = $"{baseUrl}/drugs/{item.ImageName}";
            return Ok(model);
        }

        [HttpGet("{id}:int"), Authorize]
        public async Task<IActionResult> GetItemsWithCategoryId(int id)
        {
            var items = await _item.GetListBy(a => a.CategoryId == id);
            var model = _Mapper.Map<List<ItemModel>>(items);

            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            model.ForEach(
                item =>
                {
                    item.ImageName = $"{baseUrl}/drugs/{item.ImageName}";
                });
            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> UploadItemImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is null or empty.");
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "drugs", file.FileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            string imageUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/drugs/{file.FileName}";
            return Ok(imageUrl);
        }
    }
}
