using Clean_Architecture.Application.Common.Interfaces;

namespace Clean_Architecture.Application.Authorization.Resources.Queries.GetResources;

public class GetResourcesQuery : IRequest<List<ResourceVm>>;

public class GetResourcesQueryHandler : IRequestHandler<GetResourcesQuery, List<ResourceVm>>
{
    private readonly IApplicationDbContext _context;
    
    private readonly IMapper _mapper;

    public GetResourcesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ResourceVm>> Handle(GetResourcesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Resources
            .Include(r => r.Permissions)
            .ProjectTo<ResourceVm>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
