using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fiorello.Data;
using Fiorello.Models;
using Fiorello.ViewModels;
using Fiorello.Services.Interfaces;

namespace Fiorello.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly ISliderService _sliderService;
        public SliderViewComponent(AppDbContext context, ISliderService sliderService)
        {
            _context = context;
            _sliderService = sliderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Slider> sliders = await _sliderService.GetAllByStatusAsync();
            SlidersInfo sliderInfo = await _context.slidersInfos.OrderByDescending(m=>m.Id).FirstOrDefaultAsync();

            SliderVM sliderVM = new() 
            {
                Sliders = sliders,
                SliderInfo = sliderInfo
            };

            return await Task.FromResult(View(sliderVM));
        }
    }
}
