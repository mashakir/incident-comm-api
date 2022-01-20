namespace Incident.Comm.Integration.Api.Config
{
    public class CachingSection
    {
        public bool UseRedis { get; set; }
        public string CacheKeyEnvironmentPrefix { get; set; }
    }
}
