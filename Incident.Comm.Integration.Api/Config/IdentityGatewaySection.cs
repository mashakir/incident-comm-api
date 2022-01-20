
namespace Incident.Comm.Integration.Api.Config
{
    public class IdentityGatewaySection
    {
        /// <summary>
        /// Gets or sets the base url of the Identity Gateway service.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the mobile API as configured in the Identity Gateway.
        /// </summary>
        public string ThisApiScope { get; set; }
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
