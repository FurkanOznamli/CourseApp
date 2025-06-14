﻿using CourseApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly DataContext _context;
        public OgrenciController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Ogrenciler.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogrenci model)
        {
            _context.Ogrenciler.Add(model);
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

            var ogr = await _context.Ogrenciler.Include(o=>o.KursKayitlari).ThenInclude(o=>o.Kurs).FirstOrDefaultAsync(o=> o.OgrenciId==id);
            //var ogr = await _context.Ogrenciler.FirstOrDefaultAsync(o=> o.OgrenciId == id);

            if (ogr == null) 
            { 
                return NotFound();
            }

            return View(ogr);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ogrenci model)
        {
            if (id != model.OgrenciId)
            {
                return NotFound();
            }

            if (ModelState.IsValid) 
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool v = _context.Ogrenciler.Any(o => o.OgrenciId == model.OgrenciId);
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
            if(id == null)
            {
                return NotFound();
            }
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if(ogrenci==null)
            {
                return NotFound();
            }

            return View(ogrenci);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if(ogrenci==null) 
            {
                return NotFound();
            }    
            _context.Ogrenciler.Remove(ogrenci);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
