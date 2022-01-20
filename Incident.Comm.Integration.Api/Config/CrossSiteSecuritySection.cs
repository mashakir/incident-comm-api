namespace Incident.Comm.Integration.Api.Config
{
    public class CrossSiteSecuritySection
    {
        public bool DisableCrossSiteSecurity { get; set; }
        public string CsrfCookieName { get; set; }
        public string SharedSecret { get; set; }
        public string CsrfHeaderName { get; set; }
    }
}
