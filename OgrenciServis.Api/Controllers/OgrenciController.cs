using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models;
using OgrenciServis.Models.DTO;

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

        public ActionResult<OgrenciDto> GetOgrenci(int id)
        {
            var ogrenciDto = this.ogrenci.OgrenciGetirById(id);
            if (ogrenciDto == null)
            {
                return NotFound($"ögrenci ID {id} bulunamadı.");
            }
            return Ok(ogrenciDto);
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
        public ActionResult<Ogrenci> PutOgrenci(int id,[FromBody] Ogrenci ogrenci)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var guncellenenOgrenci = this.ogrenci.OgrenciGuncelle(id, ogrenci);

            if(guncellenenOgrenci == null)

            { 
                return NotFound($"Ögrenci ID {id} bulunamdı.");
            }

            return Ok(guncellenenOgrenci);


        }

        //Delete

        [HttpDelete("{id}")]

        public ActionResult DeleteOgrenci(int id)
        {
            var silindi =this.ogrenci.OgrenciSil(id);

            if (!silindi)
            {
                return NotFound($"Ögrenci ID {id} bulunamadı");
            }
            return NoContent();

        }

        
        
        
    }
}
