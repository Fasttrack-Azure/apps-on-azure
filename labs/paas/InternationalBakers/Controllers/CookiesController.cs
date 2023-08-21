using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InternationalBakers.Data;
using InternationalBakers.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.Distributed;

namespace InternationalBakers.Controllers
{
    public class CookiesController : Controller
    {
        private readonly sbazuresqldb286930812Context _context;
        private readonly IDistributedCache _cache;

        public CookiesController(sbazuresqldb286930812Context context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Cookies
        public async Task<IActionResult> Index()
        {
            List<Cookie> cookies;
            var cachedCookies = _cache.GetString("cookiesList");

            if (!string.IsNullOrEmpty(cachedCookies))
            {
                cookies = JsonConvert.DeserializeObject<List<Cookie>>(cachedCookies);
            }
            else
            {
                cookies = _context.Cookies.ToList();
                _cache.SetString("cookieList", JsonConvert.SerializeObject(cookies));
            }

            return View(cookies);
        }

        // GET: Cookies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cookies == null)
            {
                return NotFound();
            }

            var cookie = await _context.Cookies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cookie == null)
            {
                return NotFound();
            }

            return View(cookie);
        }

        // GET: Cookies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cookies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImageUrl,Price")] Cookie cookie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cookie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cookie);
        }

        // GET: Cookies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cookies == null)
            {
                return NotFound();
            }

            var cookie = await _context.Cookies.FindAsync(id);
            if (cookie == null)
            {
                return NotFound();
            }
            return View(cookie);
        }

        // POST: Cookies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImageUrl,Price")] Cookie cookie)
        {
            if (id != cookie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cookie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CookieExists(cookie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cookie);
        }

        // GET: Cookies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cookies == null)
            {
                return NotFound();
            }

            var cookie = await _context.Cookies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cookie == null)
            {
                return NotFound();
            }

            return View(cookie);
        }

        // POST: Cookies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cookies == null)
            {
                return Problem("Entity set 'sbazuresqldb286930812Context.Cookies'  is null.");
            }
            var cookie = await _context.Cookies.FindAsync(id);
            if (cookie != null)
            {
                _context.Cookies.Remove(cookie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CookieExists(int id)
        {
          return _context.Cookies.Any(e => e.Id == id);
        }
    }
}
