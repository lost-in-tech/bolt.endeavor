namespace Bolt.Endeavor.Extensions.Bus;

public interface IEventHandler<in TEvent>
{
    Task Handle(IBusContextReader context, TEvent @event);
    bool IsApplicable(IBusContextReader context, TEvent @event);
}

public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
{
    public abstract Task Handle(IBusContextReader context, TEvent @event);

    public virtual bool IsApplicable(IBusContextReader context, TEvent @event) => true;
}