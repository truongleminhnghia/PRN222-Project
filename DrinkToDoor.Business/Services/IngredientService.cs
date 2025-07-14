
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.enums;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<IngredientService> _logger;

        public IngredientService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IngredientService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<bool> CreateAsync(IngredientRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("Id không được bỏ trống");
                }
                var ingredient = await _unitOfWork.Ingredients.FindById(id);
                if (ingredient == null)
                {
                    throw new ArgumentException($"Nguyên liệu không tồn tại với ID {id}");
                }
                if (ingredient.Status == EnumStatus.INACTIVE)
                {
                    throw new ArgumentException($"Nguyên liệu với ID {id} đã bị tạm dừng");
                }
                ingredient.Status = EnumStatus.INACTIVE;
                await _unitOfWork.Ingredients.UpdateAsync(ingredient);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<Tuple<IEnumerable<IngredientResponse>, int>> GetAsync(string? name, int pageCurrent, int pageSize)
        {
            try
            {
                var result = await _unitOfWork.Ingredients.GetAllAsync();
                if (result == null)
                {
                    throw new ArgumentException("Danh sách rỗng");
                }
                var total = result.Count();
                var paged = result.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var pagedResponses = _mapper.Map<List<IngredientResponse>>(paged);
                return Tuple.Create<IEnumerable<IngredientResponse>, int>(pagedResponses, total);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<IngredientResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("Id không được bỏ trống");
                }
                var ingredient = await _unitOfWork.Ingredients.FindById(id);
                if (ingredient == null)
                {
                    throw new ArgumentException($"Nguyên liệu không tồn tại với ID {id}");
                }
                return _mapper.Map<IngredientResponse>(ingredient);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public Task<bool> UpdateAsync(Guid id, IngredientResponse response)
        {
            throw new NotImplementedException();
        }
    }
}