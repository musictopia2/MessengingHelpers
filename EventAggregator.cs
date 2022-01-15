﻿namespace MessengingHelpers;
public class EventAggregator : IEventAggregator
{
    private readonly object _lock = new();
    public void Clear<T>()
    {
        lock (_lock)
        {
            ListHelpersClass<T>.AsyncActions.Clear();
            ListHelpersClass<T>.RegularActions.Clear();
        }
    }
    public bool HandlerAsyncExistsFor<T>(string arguments = "")
    {
        lock (_lock)
        {
            return ListHelpersClass<T>.AsyncActions.Any(x => x.Tag == arguments && x.IsDead == false);
        }
    }
    public bool HandlerRegularExistsFor<T>(string arguments = "")
    {
        lock (_lock)
        {
            return ListHelpersClass<T>.RegularActions.Any(x => x.Tag == arguments && x.IsDead == false);
        }
    }
    public void Publish<T>(T message, string arguments = "")
    {
        BasicList<CustomRegularAction<T>> list;
        lock (_lock)
        {
            list = ListHelpersClass<T>.RegularActions.Where(xx => xx.Tag == arguments).ToBasicList();
        }
        if (list.Count > 1)
        {
            throw new CustomBasicException($"Duplicates are not allowed for Publish.  The message was {typeof(T)} and the arguments was {arguments}");
        }
        if (list.Count == 0)
        {
            throw new CustomBasicException($"There is nobody subscribing for Publish.  The message was {typeof(T)} and the arguments was {arguments}");
        }
        var item = list.Single();
        if (item.Action is null)
        {
            throw new CustomBasicException($"The subscriber never invoked an action for Publish.  The message was {typeof(T)} and the arguments was {arguments}");
        }
        item.Action.Invoke(message);
        if (item.IsDead)
        {
            lock (_lock)
            {
                ListHelpersClass<T>.RegularActions.RemoveSpecificItem(item); //because it is now dead.
            }
        }
    }
    public async Task PublishAsync<T>(T message, string arguments = "") //hopefully this simple.
    {
        BasicList<CustomAsyncAction<T>> list;
        lock (_lock)
        {
            list = ListHelpersClass<T>.AsyncActions.Where(xx => xx.Tag == arguments).ToBasicList();
        }
        if (list.Count > 1)
        {
            throw new CustomBasicException($"Duplicates are not allowed for PublishAsync.  The message was {typeof(T)} and the arguments was {arguments}");
        }
        if (list.Count == 0)
        {
            throw new CustomBasicException($"There is nobody subscribing for PublishAsync.  The message was {typeof(T)} and the arguments was {arguments}");
        }
        var item = list.Single();
        if (item.Action is null)
        {
            throw new CustomBasicException($"The subscriber never invoked an action for PublishAsync.  The message was {typeof(T)} and the arguments was {arguments}");
        }
        await item.Action.Invoke(message);
        if (item.IsDead)
        {
            lock (_lock)
            {
                ListHelpersClass<T>.AsyncActions.RemoveSpecificItem(item); //because it is now dead.
            }
        }
    }
    public void Subscribe<T>(object subscriber, Action<T> action, string tag)
    {
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        lock (_lock)
        {
            if (ListHelpersClass<T>.RegularActions.Any(xx => xx.Matches(subscriber) && xx.Tag == tag))
            {
                return;
            }
            CustomRegularAction<T> customs = new(subscriber);
            customs.Tag = tag;
            customs.Action = action;
            ListHelpersClass<T>.RegularActions.Add(customs);
        }
    }
    public void Subscribe<T>(object subscriber, Func<T, Task> action, string tag)
    {
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        lock (_lock)
        {
            if (ListHelpersClass<T>.AsyncActions.Any(xx => xx.Matches(subscriber) && xx.Tag == tag))
            {
                return;
            }
            CustomAsyncAction<T> customs = new(subscriber);
            customs.Tag = tag;
            customs.Action = action;
            ListHelpersClass<T>.AsyncActions.Add(customs);
        }
    }
    public void UnsubscribeSingle<T>(object subscriber, string tag = "")
    {
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        lock (_lock)
        {
            var lists1 = ListHelpersClass<T>.RegularActions.Where(xx => xx.Matches(subscriber) && xx.Tag == tag).ToBasicList();
            foreach (var item in lists1)
            {
                ListHelpersClass<T>.RegularActions.RemoveSpecificItem(item);
            }
            var lists2 = ListHelpersClass<T>.AsyncActions.Where(xx => xx.Matches(subscriber) && xx.Tag == tag).ToBasicList();
            foreach (var item in lists2)
            {
                ListHelpersClass<T>.AsyncActions.RemoveSpecificItem(item);
            }
        }
    }
    public void UnsubscribeAll<T>(object subscriber)
    {
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber));
        }
        lock (_lock)
        {
            var lists1 = ListHelpersClass<T>.RegularActions.Where(xx => xx.Matches(subscriber)).ToBasicList();
            foreach (var item in lists1)
            {
                ListHelpersClass<T>.RegularActions.RemoveSpecificItem(item);
            }
            var lists2 = ListHelpersClass<T>.AsyncActions.Where(xx => xx.Matches(subscriber)).ToBasicList();
            foreach (var item in lists2)
            {
                ListHelpersClass<T>.AsyncActions.RemoveSpecificItem(item);
            }
        }
    }
}