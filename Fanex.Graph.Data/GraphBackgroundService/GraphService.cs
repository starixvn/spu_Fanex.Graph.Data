using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace GraphBackgroundService
{
    public partial class GraphService : ServiceBase
    {
        private IEnumerable<IModule> modules;

        private IList<object> threadObjects = new List<object>();

        public GraphService()
        {
            InitializeComponent();

            modules = GetModules().ToList();
        }

        protected override void OnStart(string[] args)
        {
            StartModules();
        }

        protected override void OnStop()
        {
            foreach (var item in threadObjects)
            {
                (item as Thread)?.Abort();
            }
        }

        public void StartModules()
        {
            foreach (var item in modules)
            {
                RunModuleThread(item);
            }
        }

        private IEnumerable<IModule> GetModules()
        {
            var m = typeof(IModule);

            var ass = Assembly.GetAssembly(m);

            return ass.GetTypes().Where(t => t.IsPublic && m.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface).Select(Activator.CreateInstance).Cast<IModule>();
        }

        private void RunModuleThread(IModule module)
        {
            if (module.Timer != null)
            {
                var t = new Timer(_ => RunModule(module), null, 1000, module.Timer.Value);

                threadObjects.Add(t);
            }
            else
            {
                var t = new Thread(_ => RunModule(module));

                t.Start();

                threadObjects.Add(t);
            }
        }

        private void RunModule(IModule module)
        {
            try
            {
                module.Run();
            }
            catch (Exception ex)
            {
                LogFactory.LogError(ex);
            }
        }
    }
}