using BestStoreMVC_EF_CFA.Models;
using BestStoreMVC_EF_CFA.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.InteropServices;

namespace BestStoreMVC_EF_CFA.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this._context = context;
            this._environment = environment;
        }
        public IActionResult Index()
        {
            var productdata = _context.Products.OrderByDescending(p => p.ID).ToList();
            return View(productdata);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductDTO productdto)
        {
            if (productdto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image File is required");
            }
            if (!ModelState.IsValid)
            {
                return View(productdto);
            }

            
            string NewFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            NewFileName += Path.GetExtension(productdto.ImageFile!.FileName);
            string ImageFullPath = _environment.WebRootPath + "/Products/" + NewFileName;
            using(var stream=System.IO.File.Create(ImageFullPath))
            {
                productdto.ImageFile.CopyTo(stream);
            }

            Product product = new Product()
            {
                Name = productdto.Name,
                Brand = productdto.Brand,
                Catagory = productdto.Catagory,
                price = productdto.price,
                Description = productdto.Description,
                ImageFileName= NewFileName,
                CreateAt= DateTime.Now,
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if(product==null)
            {
                return RedirectToAction("Index", "Product");
            }
            //create ProductDTO from Product
            ProductDTO productdto = new ProductDTO()
            {
                Name =  product.Name,
                Brand = product.Brand,
                Catagory = product.Catagory,
                price = product.price,
                Description = product.Description,
            };
            ViewData["ProductId"] = product.ID;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreateAt"] = product.CreateAt.ToString("dd/MM/yyyy");

            return View(productdto);
        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDTO prdto)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Product");
            }
            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.ID;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreateAt"] = product.CreateAt.ToString("dd/MM/yyyy");
                return View(prdto);
            }

            string NewFileName = product.ImageFileName;
            if (prdto.ImageFile != null)
            {

                NewFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                NewFileName += Path.GetExtension(prdto.ImageFile.FileName);
                string ImageFullPath = _environment.WebRootPath + "/Products/" + NewFileName;
                using (var stream = System.IO.File.Create(ImageFullPath))
                {
                    prdto.ImageFile.CopyTo(stream);
                }
                //delete the old image
                string OldImageFullPath = _environment.WebRootPath + "/Products/" + product.ImageFileName;
                System.IO.File.Delete(OldImageFullPath);
            }

            product.Name = prdto.Name;
            product.Brand = prdto.Brand;
            product.Catagory = prdto.Catagory;
            product.price = prdto.price;
            product.Description = prdto.Description;
            product.ImageFileName = NewFileName;

            _context.SaveChanges();
            return RedirectToAction("Index", "Product");

        }
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Product");
            }
            string imagefullname = _environment.WebRootPath + "/Products/" + product.ImageFileName;
            System.IO.File.Delete(imagefullname);

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index", "Product");

        }

}
}
