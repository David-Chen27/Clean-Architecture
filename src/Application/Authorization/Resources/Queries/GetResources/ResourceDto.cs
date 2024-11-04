using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Application.Authorization.Resources.Queries.GetResources;

public class ResourceDto
{
    public int RecourceId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Resource, ResourceDto>()
                .ForMember(dest => dest.RecourceId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
