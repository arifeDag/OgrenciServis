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

    public class SinavController : ControllerBase
    {
        private readonly ISinav sinav;
        //depency Injection

        public SinavController(ISinav sinav)
        {
            this.sinav = sinav;
        }

        [HttpGet]

        public ActionResult<SinavDto> GetSinavlar()
        {
            return Ok(this.sinav.TumSinavlariListele());
        }
        [HttpGet("{id}")]
        [AllowAnonymous]

        public ActionResult<SinavDto> GetSinav(int id)
        {
            try
            {
                var sinavDto = this.sinav.SinavGetirById(id);
                return Ok(sinavDto);

            }
            catch (NotFoundException ex)
            {

                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bilinmeyen bir hata oluştu{ex.Message}{id}");
            }


        }

        //postapi

        [HttpPost]
        public ActionResult<Sinav> PostSinav([FromBody] Sinav sinav)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var yeniSinav = this.sinav.SinavEkle(sinav);
            return CreatedAtAction(nameof(GetSinav), new { id = yeniSinav.SinavId }, yeniSinav);
        }

        //put:api

        [HttpPut("{id}")]
        [AllowAnonymous]

        public ActionResult<Sinav> PutSinav(int id, [FromBody] Sinav sinav)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var guncellenenSinav = this.sinav.SinavGuncelle(id, sinav);
                return Ok(guncellenenSinav);
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

        //Delete
        [HttpDelete("{id}")]
        [AllowAnonymous]


        public ActionResult DeleteSinav(int id)
        {
            try
            {
                var silindi = this.sinav.SinavSil(id);
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
