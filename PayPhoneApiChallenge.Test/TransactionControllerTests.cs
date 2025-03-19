using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using PayPhoneApiChallenge.App.Transactions.DTOs;
using PayPhoneApiChallenge.Test;
using System.Net.Http.Json;
using System.Text;

namespace PayPhoneApiChallenge.Tests
{
    public class TransactionControllerTests : WebApplicationFactory<Program>
    {
        private readonly HttpClient _client;

        public TransactionControllerTests(MockWebAplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllTransactions_ReturnsSeededTransaction()
        {
            var response = await _client.GetAsync("/api/transaction");
            response.EnsureSuccessStatusCode();

            var transactions = await response.Content.ReadFromJsonAsync<List<TransactionDto>>();
            Assert.NotNull(transactions);
            Assert.True(transactions.Count >= 1);
            Assert.Equal(50, transactions.First().Amount);
            Assert.Equal(1, transactions.First().FromWalletId);
        }

        [Fact]
        public async Task CreateTransaction_ReturnsCreatedTransaction_WhenValid()
        {
            // Arrange
            var createTransactionDto = new CreateTransactionDto
            {
                FromWalletId = 1,
                ToWalletId = 2,
                Amount = 100
            };

            var content = new StringContent(JsonConvert.SerializeObject(createTransactionDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/transactions", content);

            // Assert
            response.EnsureSuccessStatusCode(); // Código 200-299
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Id", result); // Asegúrate de que el ID de la transacción está presente
        }

        [Fact]
        public async Task CreateTransaction_ReturnsBadRequest_WhenInvalidWallets()
        {
            // Arrange
            var createTransactionDto = new CreateTransactionDto
            {
                FromWalletId = 9999,  // No existe
                ToWalletId = 9999,    // No existe
                Amount = 100
            };

            var content = new StringContent(JsonConvert.SerializeObject(createTransactionDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/transactions", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}

public class TransactionDto
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public int FromWalletId { get; set; }
    public int ToWalletId { get; set; }
}
