namespace CanteenBillingSystem.Domain.Constants
{
    public static class EmployerContributionPolicy
    {
        public const decimal INTERNAL_MAX = 7.5m;
        public const decimal CONTRACTOR_MAX = 6m;
        public const decimal INTERN_MAX = 10m;
        public const decimal VISITOR_MAX = 0m;
        public const decimal DEFAULT_CONTRIBUTION = 0m;

        // Contribution percentage for VIP clients 1 = 100%
        public const decimal VIP_PERCENTAGE = 1m;
    }
}