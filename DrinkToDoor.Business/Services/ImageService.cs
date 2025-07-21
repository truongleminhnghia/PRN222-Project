
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ImageService> _logger;
        private readonly IMapper _mapper;

        public ImageService(IUnitOfWork unitOfWork, ILogger<ImageService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> AddImage(Image image)
        {
            try
            {
                await _unitOfWork.Images.AddAsync(image);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result == 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<bool> AddImages(Guid id, List<ImageRequest> request)
        {
            var images = new List<Image>();
            try
            {
                //var ingredient = await _unitOfWork.Ingredients.FindById(id);
                //if (ingredient == null)
                //    throw new AppException(ErrorCode.NOT_FOUND, "Ingredient not found");

                foreach (var item in request)
                {
                    var image = new Image
                    {
                        IngredientId = id,
                        Url = item.ImageUrl
                    };
                    await _unitOfWork.Images.AddAsync(image);
                    images.Add(image);
                }
                // await _unitOfWork.SaveChangesWithTransactionAsync();
                return images.Count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }


        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var image = await _unitOfWork.Images.FindByIdAsync(id);
                if (image == null) throw new Exception("Image not found");
                await _unitOfWork.Images.DeleteAsync(image);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result == 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public Task<IEnumerable<ImageResponse>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ImageResponse?> GetById(Guid id)
        {
            try
            {
                var image = await _unitOfWork.Images.FindByIdAsync(id);
                if (image == null) throw new Exception("Image not found");
                return _mapper.Map<ImageResponse>(image);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<bool> Update(Guid id, ImageRequest request)
        {
            try
            {
                var image = await _unitOfWork.Images.FindByIdAsync(id);
                if (image == null) throw new Exception("Image not found");
                if (request.ImageUrl != null) image.Url = request.ImageUrl;
                await _unitOfWork.Images.UpdateAsync(image);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result == 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }
    }
}