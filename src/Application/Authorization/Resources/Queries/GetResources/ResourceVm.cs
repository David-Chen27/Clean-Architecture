using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Application.Authorization.Resources.Queries.GetResources;

public class ResourceVm
{
    public ResourceDto? Resource { get; init; }

    public IReadOnlyCollection<PermissionDto> Permissions { get; init; } = Array.Empty<PermissionDto>();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Resource, ResourceVm>()
                .ForMember(dest => dest.Resource, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));
        }
    }
}
