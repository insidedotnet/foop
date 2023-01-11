using System.Security.Claims;

namespace Authentication
{
    public class UserClaimsResponse
    {
        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public virtual IEnumerable<Claim> Claims { get; internal set; }
    }
}