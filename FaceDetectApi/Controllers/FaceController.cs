using FaceDetectApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FaceDetectApi.Controllers
{
    [Route("api/faces")]
    [ApiController]
    public class FaceController : ControllerBase
    {
        [HttpPost("detect")]
        public async Task<IActionResult> DetectFaces(IFormFile file,
            [FromServices] IDetectFaceService service) 
        {
            var result = await service.DectFace(file);
            return Ok(new { faceFileName = result});
        }

        [HttpGet("{FileName}")]
        public async Task<IActionResult> GetFaceResult([FromRoute]string fileName)
        {
            if (System.IO.File.Exists(fileName) is false)
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(fileName);
            return File(bytes, "image/jpeg");
        }
    }
}
