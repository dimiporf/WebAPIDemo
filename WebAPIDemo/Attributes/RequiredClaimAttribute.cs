namespace WebAPIDemo.Attributes
{
    // Attribute to specify required claim for accessing methods or classes
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredClaimAttribute : Attribute
    {
        // Gets the type of the required claim
        public string ClaimType { get; }

        // Gets the value of the required claim
        public string ClaimValue { get; }

        // Constructor to initialize the required claim type and value
        public RequiredClaimAttribute(string claimType, string claimValue)
        {
            this.ClaimType = claimType;
            this.ClaimValue = claimValue;
        }
    }
}
