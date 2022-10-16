using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimeShop.Models;
using Microsoft.Extensions.Hosting;
using AnimeShop.Helpers;

namespace AnimeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ShopDBContext _context;

        public ProductsController(ShopDBContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var shopDBContext = _context.ProductsList.Include(p => p.Category);
            return View(await shopDBContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductsList == null)
            {
                TempData["ModelStatus"] = "Id=null";
                return NotFound();
            }

            var product = await _context.ProductsList
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                TempData["ModelStatus"] = "Product not found";
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Если что вместо bind напрямую делай переменные, или попробуй использовать класс сразу
        public async Task<IActionResult> Create(Product product, IFormFile ImgUrl)
        {
            if(ImgUrl!=null)
            {
                product.ImageURl = await FileUploaderHelper.Upload(ImgUrl);
            }
            product.Category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == product.CategoryId);
            if (ModelState.IsValid)
            {
                product.Discount = product.Discount / 100;
                _context.ProductsList.Add(product);
                await _context.SaveChangesAsync();
                TempData["ModelStatus"] = "Product adding";
                return RedirectToAction("Index");
            }
            TempData["ModelStatus"] = "Model is not valid";
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductsList == null)
            {
                TempData["ModelStatus"] = "Id=null";
                return NotFound();
            }

            var product = await _context.ProductsList.FindAsync(id);
            if (product == null)
            {
                TempData["ModelStatus"] = "Product=null";
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product,IFormFile ImgUrl)
        {
            if (id != product.Id)
            {
                TempData["ModelStatus"] = "Id=null";
                return NotFound();
            }
            if (ImgUrl != null)
            {
                await FileUploaderHelper.DeleteImg(product.ImageURl);
                product.ImageURl = await FileUploaderHelper.Upload(ImgUrl);

            }

            if (ModelState.IsValid)
            {
                product.Discount = product.Discount / 100;
                try
                {
                    _context.Update(product);

                    IEnumerable<OrderProducts> ordProds = _context.OrderProducts.Where(x => x.ProductId == id);
                    foreach (var item in ordProds)
                    {
                        item.TotalPrice = (int)(product.Price * product.Discount * item.Amount);
                    }
                    //_context.ProductsList.Remove(_context.ProductsList.Find(id));
                    //_context.ProductsList.Add(product);
                    TempData["ModelStatus"] = "Product edited";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductsList == null)
            {
                TempData["ModelStatus"] = "Id=null";
                return NotFound();
            }

            var product = await _context.ProductsList
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                TempData["ModelStatus"] = "Product is not found";
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductsList == null)
            {
                return Problem("Entity set 'ShopDBContext.ProductsList'  is null.");
            }
            var product = await _context.ProductsList.FindAsync(id);
            if (product != null)
            {
                _context.ProductsList.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            TempData["ModelStatus"] = "Product have benn delete";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.ProductsList.Any(e => e.Id == id);
        }
    }
}
