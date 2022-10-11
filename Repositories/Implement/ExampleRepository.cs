using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Context;

namespace Monolithic.Repositories.Implement;

public class ExampleRepository : IExampleRepository
{
    private readonly DataContext _db;

    public ExampleRepository(DataContext db)
    {
        _db = db;
    }
}