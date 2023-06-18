using Fiorello.Areas.Admin.ViewModels.Product;
using Fiorello.Areas.Admin.ViewModels.SliderInfo;
using Fiorello.Data;
using Fiorello.Helpers;
using Fiorello.Models;
using Fiorello.Services;
using Fiorello.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISettingService _settingService;
        private readonly ICategoryService _categoryService;
        private readonly IDiscountService _discountService;
        public ProductController(IProductService productService,
                                 ISettingService settingService,
                                 ICategoryService categoryService,
                                 IDiscountService discountService)
        {
            _productService = productService;
            _settingService = settingService;
            _categoryService = categoryService;
            _discountService = discountService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var settingDatas = _settingService.GetAll();

            int take = int.Parse(settingDatas["AdminProductPaginateTake"]);

            var paginatedDatas = await _productService.GetPaginatedDatasAsync(page, take);

            int pageCount = await GetCountAsync(take);

            if (page > pageCount)
            {
                return NotFound();
            }

            List<ProductVM> mappedDatas = _productService.GetMappedDatas(paginatedDatas);

            Paginate<ProductVM> result = new(mappedDatas, page, pageCount);

            return View(result);
        }

        private async Task<int> GetCountAsync(int take)
        {
            int count = await _productService.GetCountAsync();

            var result = Math.Ceiling((decimal)count / take);

            return (int)result;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Product product = await _productService.GetWithIncludesAsync((int)id);

            if (product is null) return NotFound();

            return View(_productService.GetMappedData(product));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetCategoriesAndDiscounts();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            await GetCategoriesAndDiscounts();
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            foreach (var item in request.Images)
            {
                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "Please select only image file.");
                    return View();
                }

                if (item.CheckFileSize(2000))
                {
                    ModelState.AddModelError("Image", "Please select under 200KB image");
                    return View();
                }
            }

            await _productService.CreateAsync(request); 
            return RedirectToAction(nameof(Index));
        }
        private async Task GetCategoriesAndDiscounts()
        {
            ViewBag.categories = await GetCategories();
            ViewBag.discounts = await GetDiscounts();
        }
        private async Task<SelectList> GetCategories() => new SelectList(await _categoryService.GetAll(), "Id", "Name");
        private async Task<SelectList> GetDiscounts() => new SelectList(await _discountService.GetAll(), "Id", "Name");

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) BadRequest();

            Product product = await _productService.GetWithIncludesAsync((int)id);

            if (product == null) return NotFound();

            await GetCategoriesAndDiscounts();

            var viewModel = new ProductEditVM
            {
                Name = product.Name,
                Description = product.Description, 
                Price = product.Price,
                CategoryId = product.CategoryId,
                DiscountId = (int)product.DiscountId,
                Images = product.Images.ToList(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductEditVM request, int? id)
        {
            if (id is null) return BadRequest();

            await GetCategoriesAndDiscounts();

            var product = await _productService.GetWithIncludesAsync((int)id);

            if (product == null) return NotFound();

            if (!ModelState.IsValid)
            {
                request.Images = product.Images.ToList();
                return View(request);
            }

            if (request.NewImage is null) {
                foreach (var image in request.NewImage)
                {
                    if (!image.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("NewImage", "Please select only an image file.");
                        request.Images = product.Images.ToList();
                        return View(request);
                    }

                    if (image.CheckFileSize(2000))
                    {
                        ModelState.AddModelError("NewImage", "The image size must be a maximum of 200KB.");
                        request.Images = product.Images.ToList();
                        return View(request);
                    }
                }
            }

            await _productService.EditAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            await _productService.DeleteImageByIdAsync(id);

            return Ok();
        }
    }
}
