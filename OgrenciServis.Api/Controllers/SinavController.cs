using Microsoft.AspNetCore.Mvc;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models;
using OgrenciServis.Models.DTO;

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

        public ActionResult<SinavDto> GetSinav(int id)
        {
            var sinavDto = this.sinav.SinavGetirById(id);
            if (sinavDto == null)
            {
                return NotFound($"sinav ID{id} bulunamadı.");
            }
            return Ok(sinavDto);
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
        public ActionResult<Sinav> PutSinav(int id, [FromBody] Sinav sinav)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var guncellenenSinav = this.sinav.SinavGuncelle(id, sinav);
            if (guncellenenSinav == null)
            {
                return NotFound($"Sinav ID {id}bulunamdı.");
            }

            return Ok(guncellenenSinav);
        }

        //Delete
        [HttpDelete("{id}")]


        public ActionResult DeleteSinav(int id)
        {
            var silindi = this.sinav.SinavSil(id);

            if (!silindi)
            {
                return NotFound($"Sinav ID {id} bulunamadı.");
            }
            return NoContent();
        }




    }
}
