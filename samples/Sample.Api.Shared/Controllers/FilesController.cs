using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;

namespace Sample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        public IActionResult Index()
        {
            var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(folder, "FailedTest1.png");
            const string mime = "application/octet-stream";

            return PhysicalFile(file, mime, "FailedTest1.png");
        }
    }
}