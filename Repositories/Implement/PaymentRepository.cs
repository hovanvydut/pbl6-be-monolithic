using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Context;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;

namespace Monolithic.Repositories.Implement;

public class PaymentRepository : IPaymentRepository
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;
    public PaymentRepository(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<BankCodeEntity>> GetAllBankCode()
    {
        return await _db.BankCodes.ToListAsync();
    }

    public async Task CreateVNPHistory(int userId, VNPHistoryDTO vnpHistoryDTO)
    {
        VNPHistoryEntity entity = _mapper.Map<VNPHistoryEntity>(vnpHistoryDTO);
        // TODO: hardcode
        entity.UserAccountId = userId;
        await _db.VNPHistory.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<VNPHistoryEntity> GetByTxnRef(long txtRef)
    {
        return await _db.VNPHistory.FindAsync(txtRef);
    }

    public async Task updateStatusTransaction(long txtRef, string status)
    {
        var entity = await GetByTxnRef(txtRef);
        if (entity != null) {
            entity.vnp_TransactionStatus = status;
            await _db.SaveChangesAsync();
        }
    }
}