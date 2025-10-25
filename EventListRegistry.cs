namespace MessengingHelpers;
internal static class EventListRegistry
{
    private static readonly List<IEventList> _all = [];

    public static void Register(IEventList list)
    {
        lock (_all)
        {
            _all.Add(list);
        }
    }

    public static void ClearAll()
    {
        lock (_all)
        {
            foreach (var l in _all)
            {
                l.Clear();
            }
        }
        _all.Clear(); //maybe this too (?)
    }
}