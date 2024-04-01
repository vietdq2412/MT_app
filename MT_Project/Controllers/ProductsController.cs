﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MT_app.business.Services;
using MT_app.core.Models;
using MT_app.core.ViewModel;

namespace MT_Project.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService productService;
    private readonly ISupplierService supplierService;
    private readonly ICategoryService categoryService;
    private readonly IMemoryCache cache;


    public ProductsController(IMemoryCache cache, IProductService productService,
        ISupplierService supplierService, ICategoryService categoryService
    )
    {
        this.productService = productService;
        this.supplierService = supplierService;
        this.categoryService = categoryService;
        this.cache = cache;
    }

    public async Task<IActionResult> Index()
    {
        List<Product> list = await productService.FindAll();
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
}