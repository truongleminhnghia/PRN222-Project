using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class KitService : IKitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<KitService> _logger;
        private readonly IMapper _mapper;

        public KitService(IUnitOfWork unitOfWork, ILogger<KitService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> CreateKit(KitRequest request)
        {
            try
            {
                var kitExisting = await _unitOfWork.Kits.FindByName(request.Name);
                if (kitExisting != null) throw new Exception($"Kit đã tồn tại với tên {request.Name}");
                var kit = _mapper.Map<Kit>(request);
                await _unitOfWork.Kits.Create(kit);
                if (request.IngredientIds == null) throw new Exception("Danh sách nguyên liệu được chọn không thể null");
                var kitIngredients = new List<KitIngredient>();
                foreach (var IngredientId in request.IngredientIds)
                {
                    var ingredient = await _unitOfWork.Ingredients.FindById(IngredientId);
                    if (ingredient == null) throw new Exception("Nguyên liệu không tồn tại");
                    var kitIngredient = new KitIngredient
                    {
                        IngredientId = ingredient.Id,
                        KitId = kit.Id,
                    };
                    await _unitOfWork.KitIngredients.Create(kitIngredient);
                    kitIngredients.Add(kitIngredient);
                    kit.TotalAmount += ingredient.Price;
                }
                kit.KitIngredients = kitIngredients;
                kit.Status = EnumStatus.ACTIVE;
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? true : false;
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
                var kitExisting = await _unitOfWork.Kits.FindById(id);
                if (kitExisting == null) throw new Exception($"Kit không tồn tại với {id}");
                var result = await _unitOfWork.Kits.Delete(kitExisting);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<Tuple<IEnumerable<KitResponse>, int>> GetAll(int pageCurrent, int pageSize)
        {
            try
            {
                var result = await _unitOfWork.Kits.FindAll();
                var total = result.Count();
                var paged = result.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var pagedResponses = _mapper.Map<List<KitResponse>>(paged);
                return Tuple.Create<IEnumerable<KitResponse>, int>(pagedResponses, total);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<KitResponse?> GetById(Guid id)
        {
            try
            {
                var kitExisting = await _unitOfWork.Kits.FindById(id);
                if (kitExisting == null) throw new Exception($"Kit không tồn tại với {id}");
                return _mapper.Map<KitResponse>(kitExisting);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public Task<bool> Update(Guid id, KitRequest request)
        {
            throw new NotImplementedException();
        }
    }
}