﻿namespace SaveVidReceiver.Services.Interfaces
{
    public interface IForwarder
    {
        public Task ForwardAsync(long chatId, string link);
    }
}
