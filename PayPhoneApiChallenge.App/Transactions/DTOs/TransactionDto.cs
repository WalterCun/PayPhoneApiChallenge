namespace PayPhoneApiChallenge.App.Transactions.DTOs;

public class TransactionDto
{
    public int Id { get; set; }
    public int FromWalletId { get; set; }
    public int ToWalletId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}