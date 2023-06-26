using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy_API.Controllers
{
    [Route("/[controller]/[action]")]
    [ApiController, Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IService<TbCategories> _category;
        private readonly IMapper _Mapper;

        public CategoryController(IMapper mapper, IService<TbCategories> category)
        {
            _Mapper = mapper;
            _category = category;
        }
    
        [HttpGet]
        public async Task<IActionResult> GetAllCategoreis()
        {

            var categories = await _category.GetAll();
            var model = _Mapper.Map<List<IdAndNameModel>>(categories);
            return Ok(model);
        }
    }
}
