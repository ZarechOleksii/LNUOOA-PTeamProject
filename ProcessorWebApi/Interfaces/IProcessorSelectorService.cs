namespace ProcessorWebApi.Interfaces
{
    public interface IProcessorSelectorService
    {
        public IProcessor? GetRequiredProcessor(string uri);
    }
}
