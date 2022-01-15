namespace MessengingHelpers;
public static class MessengingGlobalClass
{
    public static IEventAggregator? Aggregator { get; set; }
    public static void SetUpDefaultAggregators()
    {
        Aggregator = new EventAggregator();
    }
}