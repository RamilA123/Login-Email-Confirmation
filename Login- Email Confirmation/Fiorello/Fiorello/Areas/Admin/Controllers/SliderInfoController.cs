using Fiorello.Areas.Admin.ViewModels.Slider;
using Fiorello.Areas.Admin.ViewModels.SliderInfo;
using Fiorello.Helpers;
using Fiorello.Models;
using Fiorello.Services;
using Fiorello.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class SliderInfoController : Controller
    {
        private readonly ISliderInfoService _sliderInfoService;
        public SliderInfoController(ISliderInfoService sliderInfoService)
        {
            _sliderInfoService = sliderInfoService;
        }
        [HttpGet]
        public async Task<IActionResult> Index() => View(await _sliderInfoService.GetAllMappedDatasAsync());

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            SlidersInfo slidersInfo = await _sliderInfoService.GetByIdAsync((int)id);

            if (slidersInfo == null) return NotFound();

            return View (_sliderInfoService.GetMappedData(slidersInfo));
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderInfoCreateVM request)
        {
            if (!ModelState.IsValid) return View();
            if (!request.Images.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Please select only image file.");
                return View();
            }

            if (request.Images.CheckFileSize(2000))
            {
                ModelState.AddModelError("Image", "Please select under 2000KB image");
                return View();
            }
            SliderInfoCreateVM allRequest = new()
            {
                Images = request.Images,
                Info = request.Info,
                Title = request.Title,
            };

            await _sliderInfoService.CreateAsync(allRequest);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sliderInfoService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) BadRequest();

            SlidersInfo slidersInfo = await _sliderInfoService.GetByIdAsync((int)id);

            if (slidersInfo == null) return NotFound();

            return View(new SliderInfoEditVM {Id = slidersInfo.Id, Image = slidersInfo.SignImage, Info = slidersInfo.Info, Title = slidersInfo.Title });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SliderInfoEditVM request)
        {
            if (id is null) return BadRequest();

            SlidersInfo slidersInfo = await _sliderInfoService.GetByIdAsync((int)id);

            if(slidersInfo == null) return NotFound();

            if (request.NewImage is null) return RedirectToAction(nameof(Index));

            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "Please select only image file.");
                request.Image = slidersInfo.SignImage;
                return View(request);
            }
            if (request.NewImage.CheckFileSize(2000))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 200KB");
                request.Image = slidersInfo.SignImage;
                return View(request);
            }

            await _sliderInfoService.EditAsync(slidersInfo, request);

            return RedirectToAction(nameof(Index));
        }
    }
}
