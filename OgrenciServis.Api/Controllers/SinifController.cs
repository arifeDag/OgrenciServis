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
    public class SinifController : ControllerBase
    {
        private readonly ISinif sinif;

        //depency Injection

        public SinifController(ISinif sinif)
        {
            this.sinif = sinif;
        }

        [HttpGet]

        public ActionResult<SinifDto> GetSiniflar()
        {
            return Ok(this.sinif.TumSiniflariListele());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]

        public ActionResult<SinifDto> GetSinif(int id)
        {
            try
            {
                var SinifDto = this.sinif.SinifGetirById(id);
                return Ok(SinifDto);

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

        public ActionResult<Sinif> PostSinif([FromBody] Sinif sinif)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var yeniSinif = this.sinif.SinifEkle(sinif);
            return CreatedAtAction(nameof(GetSinif), new { id = yeniSinif.SinifId }, yeniSinif);
        }
        [HttpPut("{id}")]
        [AllowAnonymous]

        public ActionResult<Sinif> PutSinif(int id, [FromBody] Sinif sinif)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var guncellenenSinif = this.sinif.SinifGuncelle(id, sinif);
                return Ok(guncellenenSinif);

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

        public ActionResult DeleteSinif(int id)
        {
            try
            {
                var silindi = this.sinif.SinifSil(id);
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
