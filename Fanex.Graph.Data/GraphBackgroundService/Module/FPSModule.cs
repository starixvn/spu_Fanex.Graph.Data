using Fanex.Graph.Data;
using System;

namespace GraphBackgroundService.Module
{
    public class FPSModule : IModule
    {
        public int? Timer => null;

        public void Run()
        {
            Console.WriteLine(DbSessionFactory.ExecuteSingle<string>(Test.HelloApi).Data);
        }
    }
}