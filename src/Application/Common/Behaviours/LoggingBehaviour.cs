using System.Reflection;
using Clean_Architecture.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Clean_Architecture.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private const string MaskedValue = "*****";
    private readonly ILogger<TRequest> _logger;
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public LoggingBehaviour(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
    {
        _logger = logger;
        _user = user;
        _identityService = identityService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var loggingBehaviorAttributes = request.GetType().GetCustomAttributes<LoggingBehaviorAttribute>();

        if (loggingBehaviorAttributes.Any())
        {
            var userId = _user.Id ?? string.Empty;
            string? userName = string.Empty;

            var maskFields = loggingBehaviorAttributes
                .SelectMany(attr => attr.mask)
                .Select(field => field.ToLower())
                .ToHashSet();

            var maskedRequest = CreateMaskedRequest(request, maskFields);

            _logger.LogInformation($"Request: {maskedRequest}");

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }
            
            _logger.LogInformation($"TraceId: {_user.TraceId}");
            _logger.LogInformation($"User: {@userId} {@userName}", userId, userName);
            _logger.LogInformation($"From: {_user.IpAddress} {_user.UserAgent}");
        }

        return await next();
    }

    private TRequest CreateMaskedRequest(TRequest request, HashSet<string> maskFields)
    {
        var maskedRequest = Activator.CreateInstance<TRequest>();
        if (maskedRequest == null)
        {
            throw new InvalidOperationException($"Unable to create instance of type {typeof(TRequest)}");
        }

        foreach (var property in typeof(TRequest).GetProperties())
        {
            var value = property.GetValue(request);
            if (maskFields.Contains(property.Name.ToLower()) && value is string)
            {
                property.SetValue(maskedRequest, MaskedValue);
            }
            else
            {
                property.SetValue(maskedRequest, value);
            }
        }

        return maskedRequest;
    }
}
