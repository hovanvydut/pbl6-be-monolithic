using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Context;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;

namespace Monolithic.Repositories.Implement;

public class AddressRepository : IAddressRepository
{
    private readonly DataContext _db;

    public AddressRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<List<AddressProvinceEntity>> GetProvince()
    {
        return await _db.AddressProvinces.ToListAsync();
    }

    public async Task<AddressProvinceEntity> GetProvince(int id)
    {
        AddressProvinceEntity entity = await _db.AddressProvinces
                                            .Include(e => e.AddressDistricts)
                                            .FirstOrDefaultAsync(c => c.Id == id);
        return entity;
    }

    public async Task<List<AddressDistrictEntity>> GetDistrict()
    {
        return await _db.AddressDistricts.ToListAsync();
    }

    public async Task<AddressDistrictEntity> GetDistrict(int id)
    {
        AddressDistrictEntity entity = await _db.AddressDistricts
                                                .Include(p => p.AddressWards)
                                                .FirstOrDefaultAsync(p => p.Id == id);
        return entity;
    }

    public async Task<AddressWardEntity> GetAddress(int wardId)
    {
        AddressWardEntity entity = await _db.AddressWards
                                            .Include(p => p.AddressDistrict.AddressProvince)
                                            // .ThenInclude(p => p.AddressProvince)
                                            .FirstOrDefaultAsync(p => p.Id == wardId);
        return entity;
    }
}