namespace CommandService.EventProcess
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}