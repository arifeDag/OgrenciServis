using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models.DTO;
using OgrenciServis.Models;

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

        public ActionResult<OgretmenDto> GetOgretmen(int id)
        {
            var ogretmenDto = this.ogretmen.OgretmenGetirById(id);
            if (ogretmenDto == null)
            {
                return NotFound($"ögretmen ID {id} bulunamadı.");
            }
            return Ok(ogretmenDto);
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
        public ActionResult<Ogretmen> PutOgrenci(int id, [FromBody] Ogretmen ogretmen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var guncellenenOgretmen = this.ogretmen.OgretmenGuncelle(id, ogretmen);

            if (guncellenenOgretmen == null)

            {
                return NotFound($"Ögretmen ID {id} bulunamdı.");
            }

            return Ok(guncellenenOgretmen);


        }

        //Delete

        [HttpDelete("{id}")]

        public ActionResult DeleteOgretmen(int id)
        {
            var silindi = this.ogretmen.OgretmenSil(id);

            if (!silindi)
            {
                return NotFound($"Ögretmen ID {id} bulunamadı");
            }
            return NoContent();

        }


    }
}
