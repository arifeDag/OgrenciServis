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
    public class OgretmenController : ControllerBase
    {
        private readonly IOgretmen ogretmen;

        public OgretmenController(IOgretmen ogretmen)
        {
            this.ogretmen = ogretmen;
        }
        [HttpGet]
        public ActionResult<OgretmenDto> GetOgretmenler()
        {
            return Ok(this.ogretmen.TumOgretmenleriListele());
        }
        //get:api
        [HttpGet("{id}")]
        [AllowAnonymous]

        public ActionResult<OgretmenDto> GetOgretmen(int id)
        {

            try
            {
                var ogretmenDto = this.ogretmen.OgretmenGetirById(id);
                return Ok(ogretmenDto);

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
        //post:api/ogretmen
        [HttpPost]

        public ActionResult<Ogretmen> PostOgretmen([FromBody] Ogretmen ogretmen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var yeniOgretmen = this.ogretmen.OgretmenEkle(ogretmen);
            return CreatedAtAction(nameof(GetOgretmen), new { id = yeniOgretmen.OgretmenId }, yeniOgretmen);

        }
        // Put : api/Ogretmen/5
        [HttpPut("{id}")]
        [AllowAnonymous]
        public ActionResult<Ogretmen> PutOgrenci(int id, [FromBody] Ogretmen ogretmen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var guncellenenOgretmen = this.ogretmen.OgretmenGuncelle(id, ogretmen);
                return Ok(guncellenenOgretmen);

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

        public ActionResult DeleteOgretmen(int id)
        {
            try
            {
                var silindi = this.ogretmen.OgretmenSil(id);
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
