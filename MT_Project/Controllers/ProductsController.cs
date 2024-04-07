using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MT_app.business.Services;
using MT_app.core.Models;
using MT_app.core.ViewModel;
using System.IO;

namespace MT_Project.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService productService;
    private readonly ISupplierService supplierService;
    private readonly ICategoryService categoryService;
    private readonly IFirebaseStorageService firebaseStorageService;
    private readonly IMemoryCache cache;


    public ProductsController(
        IMemoryCache cache,
        IProductService productService,
        ISupplierService supplierService,
        ICategoryService categoryService,
        IFirebaseStorageService firebaseStorageService
    )
    {
        this.productService = productService;
        this.supplierService = supplierService;
        this.categoryService = categoryService;
        this.firebaseStorageService = firebaseStorageService;
        this.cache = cache;
    }

    public async Task<IActionResult> Index()
    {
        List<Product> list = await productService.FindAll();

        if (!cache.TryGetValue("categories", out List<Category> categories))
        {
            categories = await categoryService.FindAll();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            cache.Set("categories", categories, cacheEntryOptions);
        }

        ViewData["categories"] = categories;

        return View(list);
    }

    public async Task<IActionResult> Create()
    {
        if (!cache.TryGetValue("suppliers", out List<Supplier> suppliers))
        {
            suppliers = await supplierService.FindAll();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            cache.Set("suppliers", suppliers, cacheEntryOptions);
        }

        if (!cache.TryGetValue("categories", out List<Category> categories))
        {
            categories = await categoryService.FindAll();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            cache.Set("categories", categories, cacheEntryOptions);
        }

        ViewData["suppliers"] = suppliers;
        ViewData["categories"] = categories;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel productViewModel)
    {
        Product product = new Product()
        {
            Name = productViewModel.Name,
            Price = productViewModel.Price,
            SupplierId = productViewModel.SupplierId,
            Supplier = supplierService.FindById(productViewModel.SupplierId).Result,
            Categories = new List<Category>()
        };

        foreach (var categoryId in productViewModel.CategoryIds)
        {
            product.Categories.Add(categoryService.FindById(categoryId).Result);
        }

        await productService.Save(product);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(long id)
    {
        if (productService.Delete(id).IsCompleted)
        {
            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }

    public IActionResult Edit()
    {
        throw new NotImplementedException();
    }

    public IActionResult Details(long id)
    {
        Product? product = productService.FindById(id).Result;
        return View(product);
    }

    public IActionResult UploadFile()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(UploadFileViewModel productViewModel)
    {
        string link = "";
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        if (productViewModel.FormFile.Length > 0)
        {
            string fileName = productViewModel.FormFile.FileName;
            FileInfo fileInfo = new FileInfo(fileName);

            string fileNameWithPath = Path.Combine(path, fileName);

            await using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                await productViewModel.FormFile.CopyToAsync(fileStream);
                link = await firebaseStorageService.Upload(fileStream, fileName);
            }


            Console.WriteLine(link);
        }

        ViewData["link"] = link;
        return View();
    }
}