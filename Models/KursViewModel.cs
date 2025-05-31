using CourseApp.Data;
using CourseApp.Models;
using System.ComponentModel.DataAnnotations;


namespace CourseApp.Models
{
    public class KursViewModel
    {
        public int KursId { get; set; }
        public String? Baslik { get; set; }
        public int? OgretmenId { get; set; }


        public ICollection<KursKayit> KursKayitlari { get; set; } = new List<KursKayit>();
    }
}
