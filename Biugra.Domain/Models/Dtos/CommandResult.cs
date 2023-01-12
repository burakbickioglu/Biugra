namespace Biugra.Domain.Models.Dtos;

public class CommandBase<T> : IRequest<T>
{
}
public interface ICommandResult
{
    public bool IsSucceed { get; }
    public string Message { get; }
    public ICommandResult SetFailed(string message);
    public ICommandResult SetMessage(string message);
}

public class CommandResult<T> : CommandResult where T : class
{
    public T? Data { get; }

    public static CommandResult<T> GetSucceed(string message, T data)
    {
        return new CommandResult<T>(true, message, data);
    }
    public static CommandResult<T> GetSucceed(T data)
    {
        return new CommandResult<T>(true, string.Empty, data);
    }
    public static CommandResult<T> GetFailed(string? message, T? data = null)
    {
        return new CommandResult<T>(false, message ?? string.Empty, data);
    }
    public static new CommandResult<T> GetFailed(Exception? e) => GetFailed(JsonConvert.SerializeObject(e));
    public static new CommandResult<T> NotFound() => GetFailed("Not Found !", null);

    public CommandResult() : base()
    {
    }
    private CommandResult(bool isSucceed, string message, T? data) : base(isSucceed, message)
    {
        Data = data;
    }
}

public class CommandResult : ICommandResult
{
    public bool IsSucceed { get; private set; }
    public string Message { get; private set; }
    public static CommandResult GetSucceed(string message)
    {
        return new CommandResult(true, message);
    }
    public static CommandResult GetSucceed()
    {
        return new CommandResult(true, string.Empty);
    }
    public ICommandResult SetFailed(string message)
    {
        IsSucceed = false;
        Message = message;
        return this;
    }
    public ICommandResult SetMessage(string message)
    {
        Message = message;
        return this;
    }

    public static CommandResult GetFailed(string message)
    {
        return new CommandResult(false, message);
    }
    public static CommandResult GetFailed(Exception x)
    {
        return new CommandResult(false, x.Message);
    }
    public static CommandResult NotFound()
    {
        return new CommandResult(false, "Not Found");
    }
    public CommandResult()
    {
        IsSucceed = false;
        Message = string.Empty;
    }
    protected CommandResult(bool isSucceed, string message)
    {
        IsSucceed = isSucceed;
        Message = message;
    }
}
