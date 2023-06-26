
namespace Pharmacy_Web.Controllers
{
    [Authorize(Roles = AppRoles.Doctor)]
    public class RevealController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IService<VwItemsWithUnits> _items;
        private readonly IService<TbOrders> _rosheta;
        private readonly IMapper _Mapper;
        private readonly IService<VwRoshetaHead> _rohetaHead;
        private readonly IService<VwRoshetaBody> _rohetaBody;
        public RevealController(UserManager<ApplicationUser> userManager, IService<VwItemsWithUnits> items, IMapper mapper, IService<VwRoshetaHead> rohetaHead, IService<VwRoshetaBody> rohetaBody, IService<TbOrders> rosheta)
        {
            _userManager = userManager;
            _items = items;
            _Mapper = mapper;
            _rohetaHead = rohetaHead;
            _rohetaBody = rohetaBody;
            _rosheta = rosheta;
           
        }
        [HttpGet, AjaxOnly]
        public async Task<IActionResult> Add()
        {
            var model = new RevealModel
            {
                Users = await _userManager.GetUsersInRoleAsync(AppRoles.Patient),
                Medicine =HomeController.Medicines
            };
            return PartialView("_RevealForm", model);
        }
        [HttpPost, AjaxOnly, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string revealModel)
        {
            var data = JsonConvert.DeserializeObject<RevealModel>(revealModel);
            var rosheta = _Mapper.Map<TbOrders>(data);
            rosheta.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (await _rosheta.Add(rosheta))
            {
                var row = await _rohetaHead.GetObjectBy(a => a.RoshetaId == rosheta.Id);
                return PartialView("~/Views/Home/_DashboardTableRow.cshtml", row);
            }

            return BadRequest(Errors.SaveOperation);
        }
        public async Task<IActionResult> Rosheta(int roshitaId)
        {
            var model = new RoshetaModel
            {
                Head = await _rohetaHead.GetObjectBy(a => a.RoshetaId == roshitaId),
                Body = await _rohetaBody.GetListBy(a => a.OrderId == roshitaId),
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken, AjaxOnly]
        public async Task<IActionResult> DeleteRosheta(int id)
        {
            if (await _rosheta.DeleteObjectBy(a => a.Id == id))
                return Ok();
             return BadRequest(Errors.SaveOperation);
        }
    }
}