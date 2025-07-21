

using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IImageService
    {
        Task<bool> AddImage(Image image);
        Task<bool> AddImages(Guid id, List<ImageRequest> request);
        Task<ImageResponse?> GetById(Guid id);
        Task<IEnumerable<ImageResponse>> GetAll();
        Task<bool> Delete(Guid id);
        Task<bool> Update(Guid id, ImageRequest request);
    }
}