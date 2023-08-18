using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tcc_mypet_back.Data.Context;
using tcc_mypet_back.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Request;
using tcc_mypet_back.Extensions;
using tcc_mypet_back.Data.Interfaces;

namespace tcc_mypet_back.Data.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            var productImages = await _context.ProductImages.ToListAsync();
            var productImageDtos = _mapper.Map<List<ProductImageDTO>>(productImages);

            foreach (var product in productDtos)
            {
                product.ProductImages = productImageDtos.Where(pi => pi.ProductId == product.Id).ToList();
            }

            return productDtos;
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) throw new Exception("Product not found.");

            var productImages = await _context.ProductImages.Where(pi => pi.ProductId == product.Id).ToListAsync();
            var productDto = _mapper.Map<ProductDTO>(product);
            productDto.ProductImages = _mapper.Map<List<ProductImageDTO>>(productImages);

            return productDto;
        }

        public async Task<ProductDTO> CreateAsync(ProductRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = _mapper.Map<Product>(request);
                product.CreatedAt = DateTime.UtcNow;
                var productDb = await _context.Products.AddAsync(product);

                await _context.SaveChangesAsync();

                if (request.Images != null && request.Images.Count > 6)
                    throw new Exception("Cannot attach more than 6 images.");

                foreach (var file in request.Images)
                {
                    var base64Image = ImageExtensions.ConvertFileToBase64(file);
                    var productImage = new ProductImage
                    {
                        ImageName = file.FileName,
                        Image64 = base64Image,
                        ProductId = productDb.Entity.Id
                    };
                    _context.ProductImages.Add(productImage);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(product.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error creating product.", ex);
            }
        }

        public async Task<ProductDTO> UpdateAsync(int id, ProductRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null) throw new Exception("Product not found.");

                _mapper.Map(request, product);

                if (request.Images != null && request.Images.Count > 6)
                    throw new Exception("Cannot attach more than 6 images.");

                product.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var existingImages = await _context.ProductImages.Where(pi => pi.ProductId == product.Id).ToListAsync();
                _context.ProductImages.RemoveRange(existingImages); // Remove existing images

                await _context.SaveChangesAsync();

                foreach (var file in request.Images)
                {
                    var base64Image = ImageExtensions.ConvertFileToBase64(file);
                    var productImage = new ProductImage
                    {
                        ImageName = file.FileName,
                        Image64 = base64Image,
                        ProductId = product.Id
                    };
                    _context.ProductImages.Add(productImage);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(product.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error updating product.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null) throw new Exception("Product not found.");

                var existingImages = await _context.ProductImages.Where(pi => pi.ProductId == product.Id).ToListAsync();
                _context.ProductImages.RemoveRange(existingImages);

                product.DeleteAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error deleting product.", ex);
            }
        }
        public async Task<FavoriteProductDto> AddToFavoriteAsync(FavoriteProductRequest request)
        {
            var favorite = await _context.FavoriteProducts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(f => f.ProductId == request.ProductId && f.UserId == request.UserId);
            if(favorite != null)
            {
                favorite.DeleteAt = null;
                favorite.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                favorite = _mapper.Map<FavoriteProduct>(request);
                favorite.CreatedAt = DateTime.UtcNow;
                await _context.FavoriteProducts.AddAsync(favorite);
            }
                await _context.SaveChangesAsync();

            return _mapper.Map<FavoriteProductDto>(favorite);
        }

        public async Task RemoveFromFavoritesAsync(int productId, int userId)
        {
            var favorite = await _context.FavoriteProducts
                            .FirstOrDefaultAsync(f => f.ProductId == productId && f.UserId == userId);

            if (favorite == null)
                throw new Exception("Favorite product not found.");

            favorite.DeleteAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<ReportedProductDto> ReportProductAsync(ReportedProductRequest request)
        {
            var reported = await _context.ReportedProducts
            .FirstOrDefaultAsync(r => r.ProductId == request.ProductId);
            
            if (reported != null)
            {
                reported.Counter += 1;
            }
            else
            {
                reported = new ReportedProduct { ProductId = request.ProductId, Counter = 1 };
                reported.CreatedAt = DateTime.UtcNow;
                await _context.ReportedProducts.AddAsync(reported);
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<ReportedProductDto>(reported);
        }

        public async Task UnreportProductAsync(int productId)
        {
            var reported = await _context.ReportedProducts.FirstOrDefaultAsync(r => r.ProductId == productId);

            if (reported == null)
                throw new Exception("Reported product not found.");

            reported.Counter -= 1;
            if(reported.Counter == 0)
            {
                _context.ReportedProducts.Remove(reported);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<List<FavoriteProductDto>> GetAllFavoriteProductsAsync()
        {
            var favorites = await _context.FavoriteProducts.ToListAsync();
            return _mapper.Map<List<FavoriteProductDto>>(favorites);
        }

        public async Task<List<FavoriteProductDto>> GetFavoriteProductsByUserIdAsync(int userId)
        {
            var favorites = await _context.FavoriteProducts
                            .Where(f => f.UserId == userId)
                            .ToListAsync();

            return _mapper.Map<List<FavoriteProductDto>>(favorites);
        }

    }
}
