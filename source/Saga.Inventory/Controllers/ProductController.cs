using Microsoft.AspNetCore.Mvc;
using Saga.Common.Controller;

namespace Saga.Inventory.Controllers
{
    public class ProductController : BaseController
    {
        public ProductController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetIndex()
        {
            return Ok();
        } 
    }
}
