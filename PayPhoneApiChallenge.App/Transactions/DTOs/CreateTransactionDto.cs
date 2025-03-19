using System.ComponentModel.DataAnnotations;

namespace PayPhoneApiChallenge.App.Transactions.DTOs;

public class CreateTransactionDto
{
    [Required(ErrorMessage = "El campo FromWalletId es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "FromWalletId debe ser un número positivo.")]
    public int FromWalletId { get; set; }

    [Required(ErrorMessage = "El campo ToWalletId es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "ToWalletId debe ser un número positivo.")]
    public int ToWalletId { get; set; }

    [Required(ErrorMessage = "El campo Amount es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
    public decimal Amount { get; set; }
    
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FromWalletId == ToWalletId)
        {
            yield return new ValidationResult(
                "FromWalletId y ToWalletId no pueden ser iguales.",
                [nameof(FromWalletId), nameof(ToWalletId)]
            );
        }
    }
    
}