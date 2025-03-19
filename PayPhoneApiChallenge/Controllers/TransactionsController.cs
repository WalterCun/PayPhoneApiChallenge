using Microsoft.AspNetCore.Mvc;
using PayPhoneApiChallenge.App.Transactions.DTOs;
using PayPhoneApiChallenge.App.Transactions.Services;

    
namespace PayPhoneApiChallenge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(TransactionService transactionsService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TransactionDto>), 200)]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
    {
        var transactions = await transactionsService.GetAllAsync();
        return Ok(transactions);
    }

    // POST: api/Transaction
    [HttpPost]
    [ProducesResponseType(typeof(TransactionDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TransactionDto>> CreateTransaction(
        [FromBody] CreateTransactionDto transactionDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState); // Devuelve los errores de validación al cliente
        
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