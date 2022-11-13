using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;
using Monolithic.Models.Common;
using Monolithic.Extensions;

namespace Monolithic.Repositories.Implement;

public class PaymentHistoryRepository : IPaymentHistoryRepository
{
    private readonly DataContext _db;
    public PaymentHistoryRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<PagedList<PaymentHistoryEntity>> GetWithParams(int hostId, PaymentHistoryParams paymentHistoryParams)
    {
        var histories = _db.PaymentHistories.Include(b => b.HostAccount)
                            .OrderByDescending(c => c.CreatedAt)
                            .Where(p => p.Post.DeletedAt == null);

        if (hostId > 0)
        {
            histories = histories.Where(h => h.HostId == hostId);
        }

        if (paymentHistoryParams.FromDate != null && paymentHistoryParams.ToDate != null)
        {
            histories = histories.Where(h => paymentHistoryParams.FromDate <= h.CreatedAt &&
                                            h.CreatedAt <= paymentHistoryParams.ToDate);
        }

        if (!String.IsNullOrEmpty(paymentHistoryParams.SearchValue))
        {
            var searchValue = paymentHistoryParams.SearchValue.ToLower();
            histories = histories.Where(
                h => h.HostAccount.Email.ToLower().Contains(searchValue) ||
                     h.PaymentCode.ToLower().Contains(searchValue) ||
                     h.Description.ToLower().Contains(searchValue)
            );
        }
        return await histories.ToPagedList(paymentHistoryParams.PageNumber, paymentHistoryParams.PageSize);
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