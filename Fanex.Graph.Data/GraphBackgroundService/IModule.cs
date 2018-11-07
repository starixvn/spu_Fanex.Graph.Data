namespace GraphBackgroundService
{
    public interface IModule
    {
        void Run();

        int? Timer { get; }
    }
}