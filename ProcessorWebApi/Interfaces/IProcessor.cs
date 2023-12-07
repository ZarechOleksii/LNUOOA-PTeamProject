using ProcessorWebApi.Models;

namespace ProcessorWebApi.Interfaces
{
    public interface IProcessor
    {
        public Task SendMediaAsync(GetMediaRequest request);
    }
}
