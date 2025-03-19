using System.ComponentModel.DataAnnotations;

namespace PayPhoneApiChallenge.App.Wallets.DTOs;

public class CreateWalletDto
{

    [Required(ErrorMessage = "El campo DocumentId es obligatorio.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "El DocumentId debe tener entre 5 y 20 caracteres.")]
    public string DocumentId { get; set; }
    [Required(ErrorMessage = "El campo Name es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "El campo Balance es obligatorio.")]
    [Range(0, double.MaxValue, ErrorMessage = "El balance debe ser igual o mayor a 0.")]
    public decimal Balance { get; set; }
}