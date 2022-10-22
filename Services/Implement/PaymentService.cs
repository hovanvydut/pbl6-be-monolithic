using AutoMapper;
using Monolithic.Models.DTO;
using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;

namespace Monolithic.Services.Implement;

public class PaymentService : IPaymentService
{
    private readonly IBankCodeRepository _bankCodeRepo;
    private readonly IMapper _mapper;
    public PaymentService(IBankCodeRepository bankCodeRepo, IMapper mapper)
    {
        _bankCodeRepo = bankCodeRepo;
        _mapper = mapper;
    }
    
    public async Task<List<BankCodeDTO>> GetAllBankCode()
    {
        var bankCodeEntityList = await _bankCodeRepo.GetAll();
        return bankCodeEntityList.Select(b => _mapper.Map<BankCodeDTO>(b)).ToList();
    }
}