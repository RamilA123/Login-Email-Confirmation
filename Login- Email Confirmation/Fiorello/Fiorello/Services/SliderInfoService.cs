using Fiorello.Areas.Admin.ViewModels.SliderInfo;
using Fiorello.Data;
using Fiorello.Helpers;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Services
{
    public class SliderInfoService : ISliderInfoService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderInfoService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<List<SliderInfoVM>> GetAllMappedDatasAsync()
        {
            List<SliderInfoVM> list = new();

            List<SlidersInfo> slidersInfos = await GetAllAsync();

            foreach (SlidersInfo sliderInfo in slidersInfos)
            {
                SliderInfoVM model = new()
                {
                    Id = sliderInfo.Id,
                    Info = sliderInfo.Info,
                    SignImage = sliderInfo.SignImage,
                    Title = sliderInfo.Title
                };
                list.Add(model);
            }
            return list;
        }
        public async Task<List<SlidersInfo>> GetAllAsync() => await _context.slidersInfos.ToListAsync();
        public async Task<SlidersInfo> GetByIdAsync(int id) => await _context.slidersInfos.FirstOrDefaultAsync(x => x.Id == id);
        public SliderInfoDetailVM GetMappedData(SlidersInfo slidersInfo)
        {
            SliderInfoDetailVM model = new()
            {
                Info = slidersInfo.Info,
                SignImage = slidersInfo.SignImage,
                Title = slidersInfo.Title,
                CreateDate = slidersInfo.CreatedDate.ToString("dd/MM/yyyy"),
            };

            return model;
        }
        public async Task CreateAsync(SliderInfoCreateVM sliderInfoCreate)
        {
            string fileName = Guid.NewGuid().ToString() + "_" + sliderInfoCreate.Images.FileName;

            await sliderInfoCreate.Images.SaveFileAsync(fileName, _env.WebRootPath, "img");

            SlidersInfo slidersInfo = new()
            {
                SignImage = fileName,
                Title = sliderInfoCreate.Title,
                Info = sliderInfoCreate.Info,
            };

            await _context.AddAsync(slidersInfo);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            SlidersInfo slidersInfo = await GetByIdAsync(id);

            _context.slidersInfos.Remove(slidersInfo);

            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath, "img", slidersInfo.SignImage);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);    
            }
        }
        public async Task EditAsync(SlidersInfo slidersInfo, SliderInfoEditVM model)
        {
            var newImage = model.NewImage;
            string oldPath = Path.Combine(_env.WebRootPath, "img", slidersInfo.SignImage);

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;

            await newImage.SaveFileAsync(fileName, _env.WebRootPath, "img");
            
                slidersInfo.SignImage = fileName;
                slidersInfo.Info = model.Info;
                slidersInfo.Title = model.Title;
          

            await _context.SaveChangesAsync();
        }
    }
}
