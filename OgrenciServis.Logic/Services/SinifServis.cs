using OgrenciServis.DataAccess;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models;
using OgrenciServis.Models.DTO;
using OgrenciServis.Models.Exceptions;

namespace OgrenciServis.Logic.Services
{
    public class SinifServis : ISinif

    {
        private readonly OkulContext _context;


        public SinifServis(OkulContext context)
        {
            _context = context;
        }


        public Sinif SinifEkle(Sinif sinif)
        {

            try
            {
                _context.Siniflar.Add(sinif);
                _context.SaveChanges();
                return sinif;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public SinifDto? SinifGetirById(int id)
        {
            try
            {
                var sonuc = (from sinif in _context.Siniflar
                             where sinif.SinifId == id
                             select new SinifDto
                             {
                                 SinifId = sinif.SinifId,
                                 Sube = sinif.Sube,
                                 SinifNo = sinif.SinifNo
                             }).FirstOrDefault();

                if (sonuc != null)
                {
                    throw new NotFoundException("Sinif", id);

                }


                return sonuc;

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

        public Sinif? SinifGuncelle(int id, Sinif sinif)
        {
            try
            {
                var mevcutSinif = _context.Siniflar.Find(id);

                if (mevcutSinif == null)
                {
                    throw new NotFoundException("Sinif ", id);
                }

                mevcutSinif.SinifId = sinif.SinifId;
                mevcutSinif.Sube = sinif.Sube;
                mevcutSinif.SinifNo = sinif.SinifNo;

                _context.SaveChanges();
                return mevcutSinif;
            }

            catch (NotFoundException)
            {
                //bulunamadı demek ki tekrar fırlat
                throw;
            }


            catch (Exception ex)
            {

                throw new Exception("Beklenmeyen bir hata oluştu", ex);
            }
        }



        public bool SinifSil(int id)
        {
            try
            {
                var sinif = _context.Siniflar.Find(id);
                if (sinif == null)
                {

                    throw new NotFoundException("Sinif", id);
                }

                _context.Siniflar.Remove(sinif);
                _context.SaveChanges();
                return true;

            }
            catch (NotFoundException)
            {
                //bulunamadı demek ki tekrar fırlat
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Beklenmeyen bir hata oluştu", ex);
            }
        }

        public IEnumerable<SinifDto> TumSiniflariListele()
        {
            try
            {
                var sonuc = from sinif in _context.Siniflar

                            select new SinifDto
                            {
                                SinifId = sinif.SinifId,
                                Sube = sinif.Sube,
                                SinifNo = sinif.SinifNo
                            };
                return sonuc.ToList();


            }
            catch (Exception)
            {

                throw;
            };
        }
    }
}
