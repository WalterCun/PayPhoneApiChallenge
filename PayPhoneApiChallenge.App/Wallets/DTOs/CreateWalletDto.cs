namespace PayPhoneApiChallenge.App.Wallets.DTOs;

public class CreateWalletDto
{
    public string DocumentId { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
}