using Microsoft.AspNetCore.Mvc;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models.DTO;

namespace OgrenciServis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("login")]

        public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto loginRequest)
        {
            //bilgileri dogrula 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // login islwmi yap

            var result = this.authService.Login(loginRequest);

            if (result == null)
                return Unauthorized("Geçersiz kullanıcı adı veya sifre ");


            //token bilgilerini dön
            return Ok(result);
        }
    }
}
