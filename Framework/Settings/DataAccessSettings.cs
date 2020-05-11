using Framework.Interfaces.Settings;

namespace Framework.Settings
{
    public class DataAccessSettings : IDataAccessSettings
    {
        public EndpointUrlSettings EndpointUrlSettings { get; set; }
        public IoSettings IoSettings { get; set; }
    }
}
