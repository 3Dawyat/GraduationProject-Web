using BL.Filters;
using System.Security.Claims;

namespace Pharmacy_API.Controllers
{
    [Route("/[controller]/[action]")]
    [ApiController, Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IService<TbOrders> _order;
        private readonly IService<VwOrderHead> _vwOrder;
        private readonly IService<VwOrderItem> _vwOrderItems;
        private readonly IService<VwItemsWithUnits> _vwItems;
        private readonly IMapper _Mapper;
        public OrderController(IService<TbOrders> order, IMapper mapper, IService<VwOrderHead> userOrder, IService<VwOrderItem> orderItems, IService<VwItemsWithUnits> vwItems)
        {
            _order = order;
            _Mapper = mapper;
            _vwOrder = userOrder;
            _vwOrderItems = orderItems;
            _vwItems = vwItems;
        }
        [HttpPost, ValidateModelState]
        public async Task<IActionResult> AddOrder([FromBody] InvoiceHead model)
        {
            var order = _Mapper.Map<TbOrders>(model);

            var result = await _order.Add(order);
            if (!result)
                return BadRequest(Errors.SaveOperation);
            return Ok();
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserOrders(string userId)
        {
            var heads = await _vwOrder.GetListBy(a => a.UserId == userId && a.OrderType == "2");
            var orderIds = heads.Select(a => a.OrderId).ToList();

            var items = await _vwOrderItems.GetListBy(a => orderIds.Contains((int)a.OrderId!));
            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";


            var userOrders = heads
                .Where(head => orderIds.Contains(head.OrderId))
                .Select(head => new UserOrdersModel
                {
                    Date = (DateTime)head.Date!,
                    OrderId = head.OrderId,
                    Items = items.Where(item => item.OrderId == head.OrderId).ToList()
                })
                .ToList();

            userOrders.ForEach(
                item =>
                {
                    item.Items.ForEach(a => a.ImageName = $"{baseUrl}/drugs/{a.ImageName}");
                });
            return Ok(userOrders);
        }
        [HttpGet("{roshetaId:int}")]
        public async Task<IActionResult> GetRosheta(int roshetaId)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var head = await _vwOrder.GetObjectBy(a => a.OrderId == roshetaId && a.OrderType == "1");
            var items = await _vwOrderItems.GetListBy(a => a.OrderId == roshetaId);
            if (head == null || items.Count == 0)
            {
                return NotFound($"No Rosheta With Id {roshetaId}");
            }
            var itemIds = items.Select(a => a.ItemUnitId).ToList();
            var itemsWithPrice = await _vwItems.GetListBy(a => itemIds.Contains(a.ItemUnitId));
            itemsWithPrice.ForEach(item =>
            {
                var orderItem = items.First(a => a.ItemUnitId == item.ItemUnitId);
                if (orderItem != null)
                {
                    orderItem.Price = item.SalesPrice;
                    orderItem.Total = item.SalesPrice * orderItem.Qty;
                }
                item.ImageName = $"{baseUrl}/drugs/{item.ImageName}";
            });
            items.ForEach(item =>
            {
                item.ImageName = $"{baseUrl}/drugs/{item.ImageName}";
            });
            var order = new UserOrdersModel
            {
                Date = head.Date!.Value,
                OrderId = head.OrderId,
                Items = items
            };
            return Ok(order);
        }
    }
}
