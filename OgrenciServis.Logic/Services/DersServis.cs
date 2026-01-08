using OgrenciServis.DataAccess;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models;
using OgrenciServis.Models.DTO;
using OgrenciServis.Models.Exceptions;

namespace OgrenciServis.Logic.Services
{
    public class DersServis : IDers
    {
        private readonly OkulContext _context;



        public DersServis(OkulContext context)
        {
            _context = context;
        }
        public Ders DersEkle(Ders ders)
        {
            try
            {
                _context.Dersler.Add(ders);
                _context.SaveChanges();
                return ders;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DersDto? DersGetirById(int id)
        {
            try
            {
                var sonuc = (from ders in _context.Dersler
                             where ders.DersId == id
                             select new DersDto
                             {
                                 DersId = ders.DersId,
                                 DersAdi = ders.DersAdi,
                                 DersSuresi = ders.DersSuresi,
                             }).FirstOrDefault();
                // bulunmadıysa 
                if (sonuc == null)
                {
                    throw new NotFoundException("Ders", id);
                }
                return sonuc;

            }
            catch (NotFoundException)
            {
                //fırlatılan hatayı yakala ve geri fırlat
                throw;
            }
            catch (Exception ex)
            {
                //başka hata geldiyse beklenmeyen olarak fırlat
                throw new Exception("Beklenmeyen bir hata oluştu", ex);
            }
        }

        public Ders? DersGuncelle(int id, Ders ders)
        {
            try
            {
                var mevcutDers = _context.Dersler.Find(id);

                if (mevcutDers == null)
                {
                    throw new NotFoundException("Ders", id);
                }

                mevcutDers.DersId = ders.DersId;
                mevcutDers.DersAdi = ders.DersAdi;
                mevcutDers.DersSuresi = ders.DersSuresi;

                _context.SaveChanges();
                return mevcutDers;


            }
            catch (NotFoundException)
            {
                //fırlatılan hatayı yakala ve geri fırlat
                throw;
            }
            catch (Exception ex)
            {
                //başka hata geldiyse beklenmeyen olarak fırlat
                throw new Exception("Beklenmeyen bir hata oluştu", ex);
            }
        }

        public bool DersSil(int id)
        {
            try
            {
                var ders = _context.Dersler.Find(id);
                if (ders == null)
                {
                    throw new NotFoundException("Ders", id);
                }

                _context.Dersler.Remove(ders);
                _context.SaveChanges();
                return true;

            }
            catch (NotFoundException)
            {
                //fırlatılan hatayı yakala ve geri fırlat
                throw;
            }
            catch (Exception ex)
            {
                //başka hata geldiyse beklenmeyen olarak fırlat
                throw new Exception("Beklenmeyen bir hata oluştu", ex);
            }
        }

        public IEnumerable<DersDto> TumDersleriListele()
        {
            try
            {
                var sonuc = from ders in _context.Dersler
                            select new DersDto
                            {
                                DersId = ders.DersId,
                                DersAdi = ders.DersAdi,
                                DersSuresi = ders.DersSuresi,

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
