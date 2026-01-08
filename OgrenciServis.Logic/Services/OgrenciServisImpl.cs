using OgrenciServis.DataAccess;
using OgrenciServis.Models;
using OgrenciServis.Models.DTO;
using OgrenciServis.Models.Exceptions;

namespace OgrenciServis.Logic.Services
{
    public class OgrenciServisImpl : Interface.IOgrenci
    {
        private readonly OkulContext _context;

        public OgrenciServisImpl(OkulContext context)
        {
            _context = context;
        }

        public Ogrenci OgrenciEkle(Ogrenci ogrenci)
        {
            try
            {
                _context.Ogrenciler.Add(ogrenci);
                _context.SaveChanges();
                return ogrenci;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public OgrenciDto? OgrenciGetirById(int id)
        {
            try
            {
                //sorgula ve Bul
                var sonuc = (from ogrenci in _context.Ogrenciler
                             join sinif in _context.Siniflar on ogrenci.SinifId equals sinif.SinifId
                             where ogrenci.OgrenciId == id
                             select new OgrenciDto
                             {
                                 OgrenciId = ogrenci.OgrenciId,
                                 Adi = ogrenci.Adi,
                                 Soyadi = ogrenci.Soyadi,
                                 DogumTarihi = ogrenci.DogumTarihi,
                                 Sube = sinif.Sube,
                                 SinifNo = sinif.SinifNo
                             }).FirstOrDefault();

                //bulunamadıysa 
                if (sonuc == null)
                {
                    //hata fırlat
                    throw new NotFoundException("Ögrenci", id);
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

        public Ogrenci? OgrenciGuncelle(int id, Ogrenci ogrenci)
        {
            try
            {
                //sorgula ve Bul
                var mevcutOgrenci = _context.Ogrenciler.Find(id);

                //bulunamadıysa
                if (mevcutOgrenci == null)
                {
                    throw new NotFoundException("Öğrenci", id);
                }

                mevcutOgrenci.Adi = ogrenci.Adi;
                mevcutOgrenci.Soyadi = ogrenci.Soyadi;
                mevcutOgrenci.DogumTarihi = ogrenci.DogumTarihi;
                mevcutOgrenci.SinifId = ogrenci.SinifId;

                _context.SaveChanges();
                return mevcutOgrenci;

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

        public bool OgrenciSil(int id)
        {
            try
            {
                //sorgula ve bul
                var ogrenci = _context.Ogrenciler.Find(id);

                //bulunamadıysa
                if (ogrenci == null)
                {
                    throw new NotFoundException("Ögrenci", id);


                }
                _context.Ogrenciler.Remove(ogrenci);
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

        public IEnumerable<OgrenciDto> TumOgrencileriListele()
        {
            try
            {
                var sonuc = from ogrenci in _context.Ogrenciler
                            join sinif in _context.Siniflar on ogrenci.SinifId equals sinif.SinifId
                            select new OgrenciDto
                            {
                                OgrenciId = ogrenci.OgrenciId,
                                Adi = ogrenci.Adi,
                                Soyadi = ogrenci.Soyadi,
                                DogumTarihi = ogrenci.DogumTarihi,
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
