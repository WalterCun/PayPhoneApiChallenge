using Microsoft.AspNetCore.Mvc;
using PayPhoneApiChallenge.App.Wallets.DTOs;
using PayPhoneApiChallenge.App.Wallets.Interfaces;

namespace PayPhoneApiChallenge.Controllers;


[ApiController]
[Route("api/[controller]")]
public class WalletsController(IWalletService walletService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var wallets = await walletService.GetAllAsync();
        return Ok(wallets);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var wallet = await walletService.GetByIdAsync(id);
        return Ok(wallet);
    }

    [HttpPost]
    [ProducesResponseType(typeof(WalletDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateWalletDto dto)
    {
        var created = await walletService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWalletDto dto)
    {
        var updated = await walletService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await walletService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}