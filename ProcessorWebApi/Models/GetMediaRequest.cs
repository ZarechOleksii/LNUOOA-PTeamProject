namespace ProcessorWebApi.Models
{
    public class GetMediaRequest
    {
        public required string MediaUri { get; set; }

        public long ChatIdentifier { get; set; }
    }
}
