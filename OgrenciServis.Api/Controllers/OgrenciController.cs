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
    public class OgrenciController : ControllerBase
    {
        private readonly IOgrenci ogrenci;

        //depency Injection
        public OgrenciController(IOgrenci ogrenci)
        {
            this.ogrenci = ogrenci;
        }

        [HttpGet]
        public ActionResult<OgrenciDto> GetOgrenciler()
        {
            return Ok(this.ogrenci.TumOgrencileriListele());
        }
        //get:api
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<OgrenciDto> GetOgrenci(int id)
        {
            try
            {
                var ogrenciDto = this.ogrenci.OgrenciGetirById(id);
                return Ok(ogrenciDto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                //String concat
                //return StatusCode(500 ,"Bilinmeyen bir hata oluştu" + ex.Message);
                //Strig interpolation
                return StatusCode(500, $"Bilinmeyen bir hata oluştu {ex.Message} {id}");

            }
        }
        //post:api/ogenci
        [HttpPost]

        public ActionResult<Ogrenci> PostOgrenci([FromBody] Ogrenci ogrenci)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var yeniOgrenci = this.ogrenci.OgrenciEkle(ogrenci);
            return CreatedAtAction(nameof(GetOgrenci), new { id = yeniOgrenci.OgrenciId }, yeniOgrenci);

        }
        // Put : api/Ogrenci/5
        [HttpPut("{id}")]
        [AllowAnonymous]
        public ActionResult<Ogrenci> PutOgrenci(int id, [FromBody] Ogrenci ogrenci)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var guncellenenOgrenci = this.ogrenci.OgrenciGuncelle(id, ogrenci);
                return Ok(guncellenenOgrenci);

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

        public ActionResult DeleteOgrenci(int id)
        {
            try
            {
                var silindi = this.ogrenci.OgrenciSil(id);
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
