namespace Business.Identity.Proxies
{
    // Hotfix-independent variant (begin)
    /*
    /// <summary>
    /// Wrapper around the <see cref="Kentico.Membership.UserStore"/> object.
    /// </summary>
    public class KenticoUserStore: Kentico.Membership.UserStore, IKenticoUserStore
    {
        public KenticoUserStore(string siteName) : base(siteName)
        {
        }

        /// <summary>
        /// Implementation of the Dispose pattern (https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose).
        /// </summary>
        /// <param name="disposing">Flag that signals an explicit method call.</param>
        void IKenticoUserStore.Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
    */
    // Hotfix-independent variant (end)
}
