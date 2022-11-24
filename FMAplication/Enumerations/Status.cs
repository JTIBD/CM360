using System.ComponentModel.DataAnnotations;

namespace FMAplication.Enumerations
{
    public enum Status
    {
        [Display(Name = "In Active")]
        InActive,
        Active,
        Pending,
        Revert,
        Rejected,
        Completed,
        NotCompleted,
        InCompleted,
        InPlace,
        NotInPlace,
        ActiveBrand = 16
    }
}