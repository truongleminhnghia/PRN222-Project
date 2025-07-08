using AutoMapper;
using DrinkToDoor.BLL.Exceptions;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.BLL.Services
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
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
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
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }


        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var image = await _unitOfWork.Images.FindByIdAsync(id);
                if (image == null) throw new AppException(ErrorCode.NOT_FOUND, "Image not found");
                await _unitOfWork.Images.DeleteAsync(image);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result == 0) return false;
                return true;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
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
                if (image == null) throw new AppException(ErrorCode.NOT_FOUND, "Image not found");
                return _mapper.Map<ImageResponse>(image);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public async Task<bool> Update(Guid id, ImageRequest request)
        {
            try
            {
                var image = await _unitOfWork.Images.FindByIdAsync(id);
                if (image == null) throw new AppException(ErrorCode.NOT_FOUND, "Image not found");
                if (request.ImageUrl != null) image.Url = request.ImageUrl;
                await _unitOfWork.Images.UpdateAsync(image);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result == 0) return false;
                return true;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }
    }
}