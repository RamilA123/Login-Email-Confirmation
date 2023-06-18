using System;
using Fiorello.Areas.Admin.ViewModels.Product;
using Fiorello.Areas.Admin.ViewModels.SliderInfo;
using Fiorello.Models;

namespace Fiorello.Services.Interfaces
{
	public interface IProductService
	{
        Task<IEnumerable<Product>> GetAllAsync();
        Task<List<Product>> GetPaginatedDatasAsync(int page, int take);
        Task<Product> GetByIdAsync(int? id);
        Task<Product> GetByIdWithImagesAsync(int? id);
        List<ProductVM> GetMappedDatas(List<Product> products);
        Task<Product> GetWithIncludesAsync(int id);
        ProductDetailVM GetMappedData(Product product);
        Task<int> GetCountAsync();
        Task CreateAsync(ProductCreateVM model);
        Task DeleteAsync(int id);
        Task EditAsync(int productId, ProductEditVM request);
        Task DeleteImageByIdAsync(int id);

    }
}

