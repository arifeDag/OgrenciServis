using Microsoft.AspNetCore.Mvc;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models;
using OgrenciServis.Models.DTO;

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

        public ActionResult<DersDto> GetDers(int id)
        {
            var DersDto = this.ders.DersGetirById(id);
            if (DersDto == null)
            {
                return NotFound($"Ders ID{id} bulunamadı");
            }
            return Ok(DersDto);
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

        public ActionResult<Sinif> PutDers(int id, [FromBody] Ders ders)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var guncelllenenDers = this.ders.DersGuncelle(id, ders);

            if (guncelllenenDers == null)
            {
                return NotFound($"Ders ID {id}bulunamdı.");

            }

            return Ok(guncelllenenDers);

        }

        //delete
        [HttpDelete("{id}")]

        public ActionResult DeleteDers(int id)
        {
            var silindi = this.ders.DersSil(id);

            if (!silindi)
            {
                return NotFound($"Ders ID {id} bulunamadı.");
            }
            return NoContent();
        }



    }
}
