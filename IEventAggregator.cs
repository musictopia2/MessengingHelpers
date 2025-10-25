namespace MessengingHelpers;
/// <summary>
/// Enables loosely-coupled publication of and subscription to events.
/// </summary>
public interface IEventAggregator
{
    //in the last version of the game package, i never did any clearing.  maybe not even needed anymore.
    //was going to remove.  however, the only problem is since this cannot be generic, then means that does need a way to clear for each of the types.  otherwise, will be hosed for future games.
    //void Clear<T>();
    //thanks to ai, was able to figure out how to truly clear all without needing to make generic.
    void ClearAll();

    void Subscribe<T>(object subscriber, Action<T> action, string tag); //can go ahead and make required since source generators will handle this anyways
    void Subscribe<T>(object subscriber, Func<T, Task> action, string tag);
    void UnsubscribeSingle<T>(object subscriber, string tag = ""); //can go ahead and make required since source generators will handle this anyways
    void UnsubscribeAll<T>(object subscriber);

    /// <summary>
    /// Searches the subscribed handlers to check if we have a handler regular for
    /// the message type supplied.
    /// </summary>
    /// <param name="arguments">Tag to search for</param>
    /// <returns>True if any handler is found, false if not.</returns>
    bool HandlerRegularExistsFor<T>(string arguments = "");
    /// <summary>
    /// Searches the subscribed handlers to check if we have a handler async for
    /// the message type supplied.
    /// </summary>
    /// <param name="arguments">Tag to search for</param>
    /// <returns>True if any handler is found, false if not.</returns>
    bool HandlerAsyncExistsFor<T>(string arguments = "");
    /// <summary>
    /// Publishes a message.
    /// Only the ones who implement the async version will get this.
    /// </summary>
    /// <param name = "message">The message instance.</param>
    /// <param name="arguments">Extra information so 2 subscribers can reuse the class but something else is needed.</param>
    /// </param>

    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PublishAsync<T>(T message, string arguments = "");
    /// <summary>
    /// Publishes a message but not async.  Only the ones who implement the non async version will get this.
    /// </summary>
    /// <param name="message">The message instance</param>
    /// <param name="arguments">Extra information so 2 subscribers can reuse the class but something else is needed.</param>
    /// </param>
    void Publish<T>(T message, string arguments = "");
    //looks like you had to include tags even for publishall because a game like countdown has more than one gameboard it needs to print but it has tags to it.
    Task PublishAllAsync<T>(T message, string arguments = "");
    void PublishAll<T>(T message, string arguments = "");
}