using Fiorello.Areas.Admin.ViewModels.SliderInfo;
using Fiorello.Models;

namespace Fiorello.Services.Interfaces
{
    public interface ISliderInfoService
    {
        Task<List<SlidersInfo>> GetAllAsync();
        Task<List<SliderInfoVM>> GetAllMappedDatasAsync();
        Task<SlidersInfo> GetByIdAsync(int id);
        SliderInfoDetailVM GetMappedData(SlidersInfo slidersInfo);
        Task CreateAsync(SliderInfoCreateVM sliderInfoCreate);
        Task DeleteAsync(int id);
        Task EditAsync(SlidersInfo slidersInfo, SliderInfoEditVM request);
    }
}
