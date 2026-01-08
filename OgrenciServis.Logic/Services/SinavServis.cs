using OgrenciServis.DataAccess;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models;
using OgrenciServis.Models.DTO;
using OgrenciServis.Models.Exceptions;

namespace OgrenciServis.Logic.Services
{
    public class SinavServis : ISinav
    {
        private readonly OkulContext _context;


        public SinavServis(OkulContext context)
        {
            _context = context;
        }
        public Sinav SinavEkle(Sinav sinav)
        {
            {
                try
                {
                    _context.Sinavlar.Add(sinav);
                    _context.SaveChanges();
                    return sinav;
                }
                catch (Exception)
                {

                    throw;
                }

            }

        }

        public SinavDto? SinavGetirById(int id)
        {
            try
            {
                var sonuc = (from sinav in _context.Sinavlar
                             join ders in _context.Dersler on sinav.DersId equals ders.DersId
                             join ogrenci in _context.Ogrenciler on sinav.OgrenciId equals ogrenci.OgrenciId
                             join ogretmen in _context.Ogretmenler on sinav.OgretmenId equals ogretmen.OgretmenId
                             where sinav.SinavId == id
                             select new SinavDto
                             {
                                 SinavId = sinav.SinavId,
                                 Not = sinav.Not,
                                 DersAdi = ders.DersAdi,
                                 DersSuresi = ders.DersSuresi,
                                 OgrenciAdi = ogrenci.Adi,
                                 OgrenciSoyadi = ogrenci.Soyadi,
                                 OgretmenAdi = ogretmen.OgretmenAdi,
                                 OgretmenSoyadi = ogretmen.OgretmenSoyadi,
                                 Brans = ogretmen.Brans
                             }).FirstOrDefault();

                if (sonuc == null)
                {
                    throw new NotFoundException("Ögrenci", id);

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

        public Sinav? SinavGuncelle(int id, Sinav sinav)
        {
            try
            {
                var mevcutSinav = _context.Sinavlar.Find(id);

                if (mevcutSinav == null)
                {
                    throw new NotFoundException("Sinav", id);
                }

                mevcutSinav.SinavId = sinav.SinavId;
                mevcutSinav.DersId = sinav.DersId;
                mevcutSinav.OgrenciId = sinav.OgrenciId;
                mevcutSinav.OgretmenId = sinav.OgretmenId;
                mevcutSinav.Not = sinav.Not;

                _context.SaveChanges();
                return mevcutSinav;

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

        public bool SinavSil(int id)
        {
            try
            {
                var sinav = _context.Sinavlar.Find(id);
                if (sinav == null)
                {
                    throw new NotFoundException("Sinav", id);


                }

                _context.Sinavlar.Remove(sinav);
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

        public IEnumerable<SinavDto> TumSinavlariListele()
        {
            try
            {
                var sonuc = from sinav in _context.Sinavlar
                            join ders in _context.Dersler on sinav.DersId equals ders.DersId
                            join ogrenci in _context.Ogrenciler on sinav.OgrenciId equals ogrenci.OgrenciId
                            join ogretmen in _context.Ogretmenler on sinav.OgretmenId equals ogretmen.OgretmenId
                            select new SinavDto
                            {
                                SinavId = sinav.SinavId,
                                Not = sinav.Not,
                                DersAdi = ders.DersAdi,
                                DersSuresi = ders.DersSuresi,
                                OgrenciAdi = ogrenci.Adi,
                                OgrenciSoyadi = ogrenci.Soyadi,
                                OgretmenAdi = ogretmen.OgretmenAdi,
                                OgretmenSoyadi = ogretmen.OgretmenSoyadi,
                                Brans = ogretmen.Brans,
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
