using Framework.Settings;

namespace Framework.Interfaces.Settings
{
    public interface IDataAccessSettings
    {
        EndpointUrlSettings EndpointUrlSettings { get; set; }
        IoSettings IoSettings { get; set; }
    }
}
