

using AutoMapper;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.BLL.Services
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

        public Task<bool> CreateIngredient(IngredientRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IngredientResponse?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PageResult<IngredientResponse>> GetIngredients(int pageCurrent, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}