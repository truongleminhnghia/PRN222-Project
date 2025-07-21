
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SupplierService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Supplier> CreateSupplier(SupplierRequest request)
        {
            try
            {
                var supplierByEmail = await _unitOfWork.Suppliers.FindByEmail(request.Email);
                var supplierByPhone = await _unitOfWork.Suppliers.FindByPhone(request.Phone);
                var supplierByName = await _unitOfWork.Suppliers.FindByName(request.Name);

                if (supplierByEmail != null || supplierByPhone != null || supplierByName != null)
                    throw new Exception("Supplier already exists.");
                var supplier = _mapper.Map<Supplier>(request);
                var rowsAffected = await _unitOfWork.Suppliers.AddSupplier(supplier);
                return rowsAffected > 0 ? supplier : null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<bool> DeleteSuplier(Guid id)
        {
            try
            {
                var supplier = await _unitOfWork.Suppliers.FindById(id);
                if (supplier == null) throw new Exception("Supplier not found");
                var result = await _unitOfWork.Suppliers.Delete(supplier);
                if (!result) return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<IEnumerable<SupplierResponse>> GetAll()
        {
            try
            {
                var suppliers = await _unitOfWork.Suppliers.FindAll();
                if (suppliers == null) throw new Exception("No suppliers found");
                return _mapper.Map<IEnumerable<SupplierResponse>>(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<SupplierResponse?> GetById(Guid id)
        {
            try
            {
                var supplier = await _unitOfWork.Suppliers.FindById(id);
                if (supplier == null) throw new Exception("Supplier not found");
                return _mapper.Map<SupplierResponse>(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        // public async Task<PageResult<SupplierResponse>> GetSuppliers(string? name, string? contactPerson, int pageCurrent, int pageSize)
        // {
        //     try
        //     {
        //         var result = await _unitOfWork.Suppliers.FindAll(name, contactPerson);
        //         if (result == null) throw new AppException(ErrorCode.LIST_EMPTY, "List empty");
        //         var pagedResult = result.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
        //         var total = result.Count();
        //         var data = _mapper.Map<List<SupplierResponse>>(pagedResult);
        //         if (data == null || !data.Any()) throw new AppException(ErrorCode.LIST_EMPTY);
        //         var pageResult = new PageResult<SupplierResponse>(data, pageSize, pageCurrent, total);
        //         return pageResult;
        //     }
        //     catch (AppException ex)
        //     {
        //         _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
        //         throw;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
        //         throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
        //     }
        // }

        public async Task<bool> UpdateSupplier(Guid id, SupplierRequest request)
        {
            try
            {
                var supplier = await _unitOfWork.Suppliers.FindById(id);
                if (supplier == null) throw new Exception("Supplier not found");
                if (request.Name != null) supplier.Name = request.Name;
                if (request.ContactPerson != null) supplier.ContactPerson = request.ContactPerson;
                if (request.Phone != null) supplier.Phone = request.Phone;
                if (request.Email != null) supplier.Email = request.Email;
                await _unitOfWork.Suppliers.Update(supplier);
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