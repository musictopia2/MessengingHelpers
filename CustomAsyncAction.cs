namespace MessengingHelpers;
internal class CustomAsyncAction<T>
{
    private readonly WeakReference _reference;
    public Func<T, Task>? Action { get; set; }
    public string Tag { get; set; } = "";
    public CustomAsyncAction(object handler)
    {
        _reference = new WeakReference(handler);
    }
    public bool Matches(object instance, string tag)
    {
        return _reference.Target == instance && Tag == tag;
    }
    public bool Matches(object instance)
    {
        return _reference.Target == instance;
    }
    public bool IsDead => _reference.Target == null;
}