using Microsoft.AspNetCore.Mvc;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models;
using OgrenciServis.Models.DTO;

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

        public ActionResult<SinifDto> GetSinif(int id)
        {
            var SinifDto = this.sinif.SinifGetirById(id);
            if (SinifDto == null)
            {
                return NotFound($"sinif ID {id} bulunamadı");
            }

            return Ok(SinifDto);
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

        public ActionResult<Sinif> PutSinif(int id, [FromBody] Sinif sinif)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var guncellenenSinif = this.sinif.SinifGuncelle(id, sinif);
            if (guncellenenSinif == null)
            {
                return NotFound($"Sinav ID {id}bulunamdı.");
            }

            return Ok(guncellenenSinif);


        }

        //delete
        [HttpDelete("{id}")]

        public ActionResult DeleteSinif(int id)
        {
            var silindi = this.sinif.SinifSil(id);

            if (!silindi)
            {
                return NotFound($"Sinif ID {id} bulunamadı.");
            }
            return NoContent();
        }




    }
}
