using Monolithic.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Monolithic.Controllers;

public class ExampleController : BaseController
{
    private readonly IExampleRepository _exampleRepo;

    private readonly IMapper _mapper;

    public ExampleController(IExampleRepository exampleRepo,
                             IMapper mapper)
    {
        _exampleRepo = exampleRepo;
        _mapper = mapper;
    }
}