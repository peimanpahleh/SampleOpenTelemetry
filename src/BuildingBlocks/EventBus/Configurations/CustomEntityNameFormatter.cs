using MassTransit.Definition;
using MassTransit.Topology;

namespace EventBus.Configurations;

public class CustomEntityNameFormatter : IEntityNameFormatter
{
    readonly IEntityNameFormatter _entityNameFormatter;
    private readonly IEndpointNameFormatter _formatter;

    public CustomEntityNameFormatter(IEntityNameFormatter entityNameFormatter)
    {
        _entityNameFormatter = entityNameFormatter;
        _formatter = KebabCaseEndpointNameFormatter.Instance;
    }

    public string FormatEntityName<T>()
    {
        const string msg = "Msg";

        var name = _entityNameFormatter.FormatEntityName<T>();

        if (name.EndsWith(msg, StringComparison.InvariantCultureIgnoreCase))
        {
            name = name.Split(':')[1];
            name = _formatter.SanitizeName(name);
        }
        else
        {
            name = name.Split(':')[1] + "Msg";
            name = _formatter.SanitizeName(name);
        }

        return name;
    }
}
