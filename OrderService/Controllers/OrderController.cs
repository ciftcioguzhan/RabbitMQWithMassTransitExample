using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        public IPublishEndpoint PublishEndpoint { get; }

        // GET: OrderController
        public OrderController(IPublishEndpoint publishEndpoint)
        {
            PublishEndpoint = publishEndpoint;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
          await  PublishEndpoint.Publish<Order>(order);
          return Ok();
        }
        // GET: OrderController/Details/5
       
    }
}
