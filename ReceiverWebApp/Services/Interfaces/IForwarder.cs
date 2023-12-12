namespace ReceiverWebApp.Services.Interfaces
{
    public interface IForwarder
    {
        public Task<HttpResponseMessage> ForwardAsync(long chatId, string link);
    }
}
