using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Monolithic.Extensions;
using Monolithic.Models.Common;
using Monolithic.Models.Context;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Models.ReqParams;
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
        if (entity != null)
        {
            entity.vnp_TransactionStatus = status;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<PagedList<VNPHistoryEntity>> GetVNPHistories(int userId, VNPParams vnpParams)
    {
        var histories = _db.VNPHistory.Include(v => v.UserAccount)
                            .OrderByDescending(v => v.CreatedAt).AsQueryable();

        if (userId > 0)
        {
            histories = histories.Where(v => v.UserAccountId == userId);
        }

        if (vnpParams.FromDate != null && vnpParams.ToDate != null)
        {
            histories = histories.Where(h => vnpParams.FromDate <= h.CreatedAt &&
                                            h.CreatedAt <= vnpParams.ToDate);
        }

        if (!String.IsNullOrEmpty(vnpParams.SearchValue))
        {
            var searchValue = vnpParams.SearchValue.ToLower();
            histories = histories.Where(
                v => v.UserAccount.Email.ToLower().Contains(searchValue) ||
                     v.vnp_BankCode.ToLower().Contains(searchValue) ||
                     v.vnp_OrderInfo.ToLower().Contains(searchValue) ||
                     v.vnp_Amount.ToString().Contains(searchValue)
            );
        }
        return await histories.ToPagedList(vnpParams.PageNumber, vnpParams.PageSize);
    }
}