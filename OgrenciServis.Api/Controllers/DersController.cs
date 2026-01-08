using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models;
using OgrenciServis.Models.DTO;
using OgrenciServis.Models.Exceptions;

namespace OgrenciServis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DersController : ControllerBase
    {
        private readonly IDers ders;

        //depency Injection

        public DersController(IDers ders)
        {
            this.ders = ders;
        }
        [HttpGet]

        public ActionResult<DersDto> GetDersler()
        {
            return Ok(this.ders.TumDersleriListele());

        }

        [HttpGet("{id}")]
        [AllowAnonymous]

        public ActionResult<DersDto> GetDers(int id)
        {
            try
            {
                var DersDto = this.ders.DersGetirById(id);
                return Ok(DersDto);

            }
            catch (NotFoundException ex)
            {

                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bilinmeyen bir hata oluştu {ex.Message} {id}");
            }


        }

        //postapi
        [HttpPost]

        public ActionResult<Ders> PostDers([FromBody] Ders ders)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var yeniDers = this.ders.DersEkle(ders);
            return CreatedAtAction(nameof(GetDers), new { id = yeniDers.DersId }, yeniDers);

        }


        [HttpPut("{id}")]
        [AllowAnonymous]

        public ActionResult<Sinif> PutDers(int id, [FromBody] Ders ders)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var guncelllenenDers = this.ders.DersGuncelle(id, ders);
                return Ok(guncelllenenDers);

            }
            catch (NotFoundException ex)

            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bilinmeyen bir hata oluştu {ex.Message}{id}");

            }



        }

        //delete
        [HttpDelete("{id}")]
        [AllowAnonymous]

        public ActionResult DeleteDers(int id)
        {
            try
            {
                var silindi = this.ders.DersSil(id);
                return NoContent();

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Bilinmeyen bir hata oluştu {ex.Message}{id}");
            }
        }



    }
}
