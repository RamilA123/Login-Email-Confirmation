using System;
using System.IO;
using Fiorello.Areas.Admin.ViewModels.Product;
using Fiorello.Data;
using Fiorello.Helpers;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await _context.products.Include(m => m.Images)
                                                                                        .Take(8).Where(m => !m.SoftDelete)
                                                                                        .ToListAsync();
        public async Task<List<Product>> GetPaginatedDatasAsync(int page,int take) => await _context.products.Include(m=>m.Images)
                                                                                             .Include(m=>m.Category)
                                                                                             .Include(m=>m.Discount)
                                                                                             .Skip((page-1)*take)
                                                                                             .Take(take)
                                                                                             .ToListAsync();
        public async Task<Product> GetByIdAsync(int? id) => await _context.products.FindAsync(id);
        public async Task<Product> GetByIdWithImagesAsync(int? id) => await _context.products.Include(m => m.Images)
                                                                                             .FirstOrDefaultAsync(m => m.Id == id);
        public List<ProductVM> GetMappedDatas(List<Product> products)
        {
            List<ProductVM> list = new();
            foreach (var product in products)
            {
                list.Add(new ProductVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price.ToString("0.####"),
                    Discount = product.Discount.Name,
                    CategoryName = product.Category.Name,
                    Image = product.Images.Where(m=>m.IsMain).FirstOrDefault().Images
                });
            }
            return list;
        }
        public async Task<Product> GetWithIncludesAsync(int id) => await _context.products.Where(m=>m.Id == id)
                                                                                          .Include(m => m.Images)
                                                                                          .Include(m => m.Category)
                                                                                          .Include(m => m.Discount)
                                                                                          .FirstOrDefaultAsync();
        public ProductDetailVM GetMappedData(Product product)
        {
            return new ProductDetailVM
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.ToString("0.#####"),
                CategoryName = product.Category.Name,
                CreateDate = product.CreatedDate.ToString("dd/MM/yyyy"),
                Discount = product.Discount.Name,
                Image = product.Images.Select(m => m.Images)
            };
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.products.CountAsync();
        }

        public async Task CreateAsync(ProductCreateVM model)
        {
            List<Image> images = new();

            foreach (var item in model.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                await item.SaveFileAsync(fileName, _env.WebRootPath, "img");

                images.Add(new Image { Images = fileName });
            }

            images.FirstOrDefault().IsMain = true;

            Product product = new()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                DiscountId = model.DiscountId,
                Images = images
            };

            await _context.products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Product product = await GetWithIncludesAsync(id);

            _context.products.RemoveRange(product);
            await _context.SaveChangesAsync();

            foreach (var item in product.Images)
            {
                string directoryPath = Path.Combine(_env.WebRootPath, "img", item.Images);

                if (System.IO.File.Exists(directoryPath))
                {
                    System.IO.File.Delete(directoryPath);
                }
            }
        }

        public async Task EditAsync(int productId, ProductEditVM request)
        {
            List<Image> images = new();

            var product = await GetByIdAsync(productId);

            if(request.NewImage != null) {
                foreach (var item in request.NewImage)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                    await item.SaveFileAsync(fileName, _env.WebRootPath, "img");
                    images.Add(new Image { Images = fileName });
                }

                await _context.images.AddRangeAsync(images);
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.DiscountId = request.DiscountId;
            product.CategoryId = request.CategoryId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteImageByIdAsync(int id)
        {
            Image image = await _context.images.FirstOrDefaultAsync(x => x.Id == id);

            _context.images.Remove(image);
            await _context.SaveChangesAsync();

            string path = Path.Combine(_env.WebRootPath,"img", image.Images);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}

