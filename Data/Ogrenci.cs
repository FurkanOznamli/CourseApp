using CourseApp.Data;

namespace CourseApp.Data
{
    public class Ogrenci
    {
        public int OgrenciId { get; set; }
        public string? OgrenciAd { get; set; }
        public string? OgrenciSoyad { get; set; }

        public string AdSoyad
        {
            get
            {
                return this.OgrenciAd + " " + this.OgrenciSoyad;
            }
        }
        public String? Eposta { get; set; }
        public String? Telefon { get; set; }

        public ICollection<KursKayit> KursKayitlari { get; set; } = new List<KursKayit>();

    }
}
