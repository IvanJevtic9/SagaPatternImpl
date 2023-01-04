using Microsoft.AspNetCore.Mvc;

namespace Saga.Common.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    { }
}
