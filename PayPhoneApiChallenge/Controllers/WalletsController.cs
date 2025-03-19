using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayPhoneApiChallenge.App.Wallets.DTOs;
using PayPhoneApiChallenge.App.Wallets.Interfaces;
using PayPhoneApiChallenge.Exceptions;

namespace PayPhoneApiChallenge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletsController(IWalletService walletService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<WalletDto>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var wallets = await walletService.GetAllAsync();
        return Ok(wallets);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(WalletDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var wallet = await walletService.GetByIdAsync(id);
        if (wallet == null)
            throw new NotFoundException("No se encontró la billetera.");
        return Ok(wallet);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(WalletDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateWalletDto dto)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("No se pudo crear la billetera.");

        var created = await walletService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(WalletDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWalletDto dto)
    {
        if (!ModelState.IsValid)
            throw new BadRequestException("No se pudo actualizar la billetera.");

        var updated = await walletService.UpdateAsync(id, dto);
        if (updated == null)
            throw new NotFoundException("No se actulizo la billetera.");
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await walletService.DeleteAsync(id);
        if (!result)
            throw new NotFoundException("No se pudo eliminar la billetera.");
        throw new NotContentException("No se pudo eliminar la billetera.");
    }
}