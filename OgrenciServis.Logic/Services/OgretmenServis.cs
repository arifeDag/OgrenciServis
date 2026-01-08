using OgrenciServis.DataAccess;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models;
using OgrenciServis.Models.DTO;
using OgrenciServis.Models.Exceptions;

namespace OgrenciServis.Logic.Services
{
    public class OgretmenServis : IOgretmen
    {
        private readonly OkulContext _context;

        public OgretmenServis(OkulContext context)
        {
            _context = context;
        }

        public Ogretmen OgretmenEkle(Ogretmen ogretmen)
        {
            try
            {
                _context.Ogretmenler.Add(ogretmen);
                _context.SaveChanges();
                return ogretmen;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public OgretmenDto? OgretmenGetirById(int id)
        {
            try
            {
                var sonuc = (from ogretmen in _context.Ogretmenler
                             join sinif in _context.Siniflar on ogretmen.Sinif equals sinif.SinifId
                             where ogretmen.OgretmenId == id
                             select new OgretmenDto
                             {
                                 OgretmenId = ogretmen.OgretmenId,
                                 Adi = ogretmen.OgretmenAdi,
                                 Soyadi = ogretmen.OgretmenSoyadi,
                                 Brans = ogretmen.Brans,
                                 Sube = sinif.Sube,
                                 SinifNo = sinif.SinifNo
                             }).FirstOrDefault();

                if (sonuc == null)
                {
                    throw new NotFoundException("Ögretmen ", id);
                }

                return sonuc;
            }
            catch (NotFoundException)
            {

                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Beklenmeyen bir hata oluştu ", ex);
            }
        }

        public Ogretmen? OgretmenGuncelle(int id, Ogretmen ogretmen)
        {
            try
            {
                var mevcutOgretmen = _context.Ogretmenler.Find(id);

                if (mevcutOgretmen == null)
                {
                    throw new NotFoundException("Ögretmen", id);
                }

                mevcutOgretmen.OgretmenAdi = ogretmen.OgretmenAdi;
                mevcutOgretmen.OgretmenSoyadi = ogretmen.OgretmenSoyadi;
                mevcutOgretmen.Brans = ogretmen.Brans;
                mevcutOgretmen.Sinif = ogretmen.Sinif;

                _context.SaveChanges();
                return mevcutOgretmen;

            }

            catch (NotFoundException)
            {

                throw;
            }
            catch (Exception ex)
            {

                throw new Exception("Beklenmeyen bir hata oluştu", ex);
            }
        }

        public bool OgretmenSil(int id)
        {

            try
            {
                var ogretmen = _context.Ogretmenler.Find(id);

                if (ogretmen == null)
                {
                    throw new NotFoundException("Ögretmen", id);

                }
                _context.Ogretmenler.Remove(ogretmen);
                _context.SaveChanges();
                return true;

            }

            catch (NotFoundException)
            {

                throw;
            }
            catch (Exception ex)
            {

                throw new Exception("Beklenmeyen bir hata oluştu", ex);
            }
        }

        public IEnumerable<OgretmenDto> TumOgretmenleriListele()
        {
            try
            {
                var sonuc = from ogretmen in _context.Ogretmenler
                            join sinif in _context.Siniflar on ogretmen.Sinif equals sinif.SinifId
                            select new OgretmenDto
                            {
                                OgretmenId = ogretmen.OgretmenId,
                                Adi = ogretmen.OgretmenAdi,
                                Soyadi = ogretmen.OgretmenSoyadi,
                                Brans = ogretmen.Brans,
                                Sube = sinif.Sube,
                                SinifNo = sinif.SinifNo
                            };

                return sonuc.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
