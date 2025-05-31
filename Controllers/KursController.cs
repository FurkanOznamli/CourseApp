using CourseApp.Data;
using CourseApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CourseApp.Controllers
{
    public class KursController : Controller
    {
        private readonly DataContext _context;
        public KursController(DataContext context)
        { 
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Kurslar.Include(k=>k.Ogretmen).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId","AdSoyad");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Kurs model)
        {
            _context.Kurslar.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurs = await _context.Kurslar
                .Include(k=> k.KursKayitlari)
                .ThenInclude(k=>k.Ogrenci)
                .Select(k=>new KursViewModel
                {
                    KursId = k.KursId,
                    Baslik = k.Baslik,
                    OgretmenId = k.OgretmenId,
                    KursKayitlari = k.KursKayitlari
                })
                .FirstOrDefaultAsync( k=> k.KursId==id );
            //var ogr = await _context.Ogrenciler.FirstOrDefaultAsync(o=> o.KursId == id);

            if (kurs == null)
            {
                return NotFound();
            }

            //var viewModel = new KursViewModel
            //{
            //    KursId = kurs.KursId,
            //    Baslik = kurs.Baslik,
            //    OgretmenId = kurs.OgretmenId
            //};

            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");
            return View(kurs);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KursViewModel model)
        {
            if (id != model.KursId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Kurs() { KursId=model.KursId , Baslik = model.Baslik,OgretmenId=model.OgretmenId});
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool v = _context.Kurslar.Any(o => o.KursId == model.KursId);
                    if (!v)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var kurs = await _context.Kurslar.FindAsync(id);
            if (kurs == null)
            {
                return NotFound();
            }

            return View(kurs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var kurs = await _context.Kurslar.FindAsync(id);
            if (kurs == null)
            {
                return NotFound();
            }
            _context.Kurslar.Remove(kurs);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
