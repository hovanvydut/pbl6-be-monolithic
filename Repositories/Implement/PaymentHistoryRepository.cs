using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;

namespace Monolithic.Repositories.Implement;

public class PaymentHistoryRepository : IPaymentHistoryRepository
{
    private readonly DataContext _db;
    public PaymentHistoryRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<List<PaymentHistoryEntity>> GetAllByHostId(int hostId)
    {
        return await _db.PaymentHistories.Where(c => c.HostId == hostId).ToListAsync();
    }

    public async Task<PaymentHistoryEntity> GetById(int id)
    {
        var paymentHistory = await _db.PaymentHistories
                                        .FirstOrDefaultAsync(c => c.Id == id);
        if (paymentHistory == null) return null;
        _db.Entry(paymentHistory).State = EntityState.Detached;
        return paymentHistory;
    }

    public async Task<PaymentHistoryEntity> Create(PaymentHistoryEntity paymentHistoryEntity)
    {
        await _db.PaymentHistories.AddAsync(paymentHistoryEntity);
        await _db.SaveChangesAsync();
        return await GetById(paymentHistoryEntity.Id);
    }
}