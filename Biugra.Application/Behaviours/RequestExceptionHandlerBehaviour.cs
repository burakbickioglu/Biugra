
using Biugra.Domain.Models.Dtos;

namespace Biugra.Service.Behaviours;

public class RequestExceptionHandlerBehaviour<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException> where TRequest : IRequest<TResponse> where TException : Exception
{
    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        string exceptionMessage = exception.Message;
        _logger.LogError(exception, "REQUEST ERROR --- RequestType : {RequestType} -- UserId : {UserId} -- Message : {message} ---  Request : {@Request}", typeof(TRequest), _userService.GetUserId(), exceptionMessage, request);

        if (typeof(TResponse).GetTypeInfo().IsGenericType)
        {
            Type genericType = typeof(TResponse);
            ConstructorInfo? constructor = genericType.GetConstructor(Type.EmptyTypes);
            object? classObject = constructor?.Invoke(new object[] { });

            MethodInfo? classMethod = genericType.GetMethod("SetFailed");
            object? magicValue = classMethod?.Invoke(classObject, new object[] { exceptionMessage });
            if (magicValue != null)
            {
                state.SetHandled((TResponse)magicValue);
            }
        }
        else
        {
            state.SetHandled((TResponse)(object)CommandResult.GetFailed(exceptionMessage));
        }

        return Task.CompletedTask;
    }

    public RequestExceptionHandlerBehaviour(ILogger<TRequest> logger, ICurrentUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _userService;
}
