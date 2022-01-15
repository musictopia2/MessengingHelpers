namespace MessengingHelpers;
//i think this version will need to be static.  otherwise, the entire event aggravation has to be generic but can't do that though.
internal static class ListHelpersClass<T>
{
    public static BasicList<CustomAsyncAction<T>> AsyncActions { get; set; } = new();
    public static BasicList<CustomRegularAction<T>> RegularActions { get; set; } = new();
    
}