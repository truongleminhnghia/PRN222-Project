using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.BLL.Interfaces
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