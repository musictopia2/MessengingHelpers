namespace MessengingHelpers;
internal class CustomRegularAction<T>
{
    private readonly WeakReference _reference;
    public Action<T>? Action { get; set; }
    public string Tag { get; set; } = "";
    public CustomRegularAction(object handler)
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