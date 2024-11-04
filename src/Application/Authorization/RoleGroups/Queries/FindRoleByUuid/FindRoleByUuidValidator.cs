using NUlid;

namespace Clean_Architecture.Application.Authorization.RoleGroups.Queries.FindRoleByUuid;

public class FindRoleByUuidValidator : AbstractValidator<FindRoleByUuidQuery>
{
    private Ulid _ulid = Ulid.NewUlid();
    
    public FindRoleByUuidValidator()
    {
        RuleFor(v => v.Uuid)
            .NotEmpty().WithMessage("UUID is required.")
            .Must(BeAValidUlid!).WithMessage("UUID must be a valid ULID.");
    }

    private bool BeAValidUlid(string uuid)
    {
        return Ulid.TryParse(uuid, out _ulid);
    }
}
