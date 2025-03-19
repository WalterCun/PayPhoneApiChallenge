using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayPhoneApiChallenge.App.Transactions.DTOs;
using PayPhoneApiChallenge.App.Transactions.Interfaces;

    
namespace PayPhoneApiChallenge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(ITransactionsService transactionsService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<TransactionDto>), 200)]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
    {
        var transactions = await transactionsService.GetAllAsync();
        return Ok(transactions);
    }

    // POST: api/Transaction
    [HttpPost]
    [Authorize]
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