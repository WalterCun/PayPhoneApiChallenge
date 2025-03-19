namespace PayPhoneApiChallenge.App.Transactions.DTOs;

public class CreateTransactionDto
{
    public int FromWalletId { get; set; }
    public int ToWalletId { get; set; }
    public decimal Amount { get; set; }
}