using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Context;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;

namespace Monolithic.Repositories.Implement;

public class BankCodeRepository : IBankCodeRepository
{
    private readonly DataContext _db;
    public BankCodeRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<List<BankCodeEntity>> GetAll()
    {
        return await _db.BankCodes.ToListAsync();
    }
}