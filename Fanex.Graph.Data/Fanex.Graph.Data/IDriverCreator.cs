namespace Fanex.Graph.Data
{
    public interface IDriverCreator
    {
        TDriver GetDriver<TDriver>(string config);
    }
}