using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Application.Authorization.Resources.Queries.GetResources;

public class PermissionDto
{
    public int PermissionId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Permission, PermissionDto>()
                .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
