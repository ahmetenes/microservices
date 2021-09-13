using PlatformService.DTOs;

namespace PlatformService.AsyncDataService
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublish pb);
    }
}