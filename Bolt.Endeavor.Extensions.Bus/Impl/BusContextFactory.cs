namespace Bolt.Endeavor.Extensions.Bus.Impl;

internal sealed class BusContextFactory : IBusContextFactory
{
    private readonly Lazy<IBusContext> _context;
    private readonly IEnumerable<IBusContextPopulator> _contextPopulators;

    public BusContextFactory(IEnumerable<IBusContextPopulator> contextPopulators)
    {
        _contextPopulators = contextPopulators;
        _context = new Lazy<IBusContext>(BuildContext);
    }

    private IBusContext BuildContext()
    {
        var context = new BusContext();

        foreach (var populator in _contextPopulators)
        {
            populator.Populate(context);
        }

        return context;
    }

    public IBusContext Create() => _context.Value;
}
