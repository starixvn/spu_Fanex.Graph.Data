namespace Fanex.Graph.Data
{
    using System;

    public interface ICommandBuilder
    {
        Type DriverType { get; }

        string BuildCommand(GraphSetting setting);
    }
}