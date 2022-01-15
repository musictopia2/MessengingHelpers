namespace MessengingHelpers;
public interface IHandleAsync<TMessage>
{
    Task HandleAsync(TMessage message);
}