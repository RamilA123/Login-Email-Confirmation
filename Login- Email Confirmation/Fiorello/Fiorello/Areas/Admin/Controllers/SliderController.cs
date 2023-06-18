using Fiorello.Areas.Admin.ViewModels.Slider;
using Fiorello.Data;
using Fiorello.Helpers;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Fiorello_backend.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly AppDbContext _context;

        public SliderController( ISliderService sliderService,AppDbContext context)
        {
            _sliderService = sliderService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await _sliderService.GetAllMappedDatasAsync());

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        { 
            if (id is null) return BadRequest();

            Slider dbSlider = await _sliderService.GetByIdAsync((int)id);

            if (dbSlider is null) return NotFound();

            return View(_sliderService.GetMappedData(dbSlider));
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
            if (!ModelState.IsValid) return View();

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

            await _sliderService.CreateAsync(request.Images);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _sliderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            Slider dbSlider = await _sliderService.GetByIdAsync((int)id);

            if (dbSlider is null) return NotFound();

            return View(new SliderEditVM { Image = dbSlider.Image });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderEditVM request)
        {
            if (id is null) return BadRequest();

            Slider dbSlider = await _sliderService.GetByIdAsync((int)id);

            if (dbSlider is null) return NotFound();

            if (request.NewImage is null) return RedirectToAction(nameof(Index));

            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "Please select only image file.");
                request.Image = dbSlider.Image;
                return View(request);
            }
            if (request.NewImage.CheckFileSize(2000))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 200KB");
                request.Image = dbSlider.Image;
                return View(request);
            }

            await _sliderService.EditAsync(dbSlider, request.NewImage);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id is null) return BadRequest();

            Slider slider = await _sliderService.GetByIdAsync((int)id);

            if (slider is null) return NotFound();

            return Ok( await _sliderService.ChangeStatusAsync(slider));
        }
    }
}
