using Framework.Interfaces.Settings;

namespace Framework.Settings
{
    public class EndpointUrlSettings : IEndpointUrlSettings
    {
        public string BoardServiceEndPoint { get; set; }

        public string ImageServiceEndPoint { get; set; }
    }
}
