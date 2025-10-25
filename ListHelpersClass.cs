namespace MessengingHelpers;
//i think this version will need to be static.  otherwise, the entire event aggravation has to be generic but can't do that though.
internal static class ListHelpersClass<T>
{
    static ListHelpersClass()
    {
        // Register the type once when it's first used.
        EventListRegistry.Register(new Clearer());
    }
    private sealed class Clearer : IEventList
    {
        public void Clear()
        {
            AsyncActions.Clear();
            RegularActions.Clear();
        }
    }
    public static BasicList<CustomAsyncAction<T>> AsyncActions { get; set; } = new();
    public static BasicList<CustomRegularAction<T>> RegularActions { get; set; } = new();
    
}