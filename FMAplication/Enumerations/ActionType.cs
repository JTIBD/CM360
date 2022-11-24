using System.ComponentModel;

namespace FMAplication.Enumerations
{
    public enum ActionType
    {
        [Description("Distribution Check Product")]
        DistributionCheckProduct,
        [Description("Facing Count Product")]
        FacingCountProduct,
        [Description("Planogram Check Product")]
        PlanogramCheckProduct,
        [Description("Price Audit Product")]
        PriceAuditProduct
    }
}
