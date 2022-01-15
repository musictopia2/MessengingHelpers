namespace MessengingHelpers;
public interface IHandle<TMessage>
{
    void Handle(TMessage message);
}