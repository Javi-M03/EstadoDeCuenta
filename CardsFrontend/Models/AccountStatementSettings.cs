using System.ComponentModel.DataAnnotations;

namespace CardsFrontend.Models;

public class AccountStatementSettings
{
    [Display(Name = "Interest percentage (%)")]
    [Range(0, 100, ErrorMessage = "Interest percentage must be between 0 and 100.")]
    public decimal InterestPercentage { get; set; }

    [Display(Name = "Minimum payment percentage (%)")]
    [Range(0, 100, ErrorMessage = "Minimum payment percentage must be between 0 and 100.")]
    public decimal MinimumPaymentPercentage { get; set; }
}
