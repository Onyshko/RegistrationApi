using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RegApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "OnlyAdminUsers")]
        public IActionResult TestAction() => Ok("test message");
    }
}
