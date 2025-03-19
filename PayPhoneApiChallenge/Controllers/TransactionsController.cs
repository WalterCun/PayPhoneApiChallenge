using Microsoft.AspNetCore.Mvc;
using PayPhoneApiChallenge.App.Transactions.DTOs;
using PayPhoneApiChallenge.App.Transactions.Services;

namespace PayPhoneApiChallenge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(TransactionService transactionsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
    {
        var transactions = await transactionsService.GetAllAsync();
        return Ok(transactions);
    }

    // POST: api/Transaction
    [HttpPost]
    public async Task<ActionResult<TransactionDto>> CreateTransaction(
        [FromBody] CreateTransactionDto transactionDto)
    {
        try
        {
            var createdTransaction = await transactionsService.CreateAsync(transactionDto);
            return CreatedAtAction(nameof(GetTransactions), new { id = createdTransaction.Id }, createdTransaction);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message }); // Manejo de errores
        }
    }
}