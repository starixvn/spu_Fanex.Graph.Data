using System;

namespace Fanex.Graph.Data
{
    public interface IDriverFactory
    {
        (Type driverType,object driver) GetDriver(string config);
    }
}