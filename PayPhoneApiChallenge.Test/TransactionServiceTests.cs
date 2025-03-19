using Microsoft.EntityFrameworkCore;

using Moq;

using PayPhoneApiChallenge.App.Transactions.Services;
using PayPhoneApiChallenge.Domain.Transactions.Entities;
using PayPhoneApiChallenge.Domain.Wallets.Entities;
using PayPhoneApiChallenge.Infra.Persistence;
using PayPhoneApiChallenge.App.Transactions.DTOs;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace PayPhoneApiChallenge.Test
{
    public class TransactionServiceTests
    {
        private readonly Mock<DbSet<Transaction>> _mockTransactionDbSet;
        private readonly Mock<DbSet<Wallet>> _mockWalletDbSet;
        private readonly PayPhoneDbContext _context;
        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _mockTransactionDbSet = new Mock<DbSet<Transaction>>();
            _mockWalletDbSet = new Mock<DbSet<Wallet>>();

            var options = new DbContextOptionsBuilder<PayPhoneDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _context = new PayPhoneDbContext(options);
            _transactionService = new TransactionService(_context);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenWalletsNotFound()
        {
            // Arrange
            var createTransactionDto = new CreateTransactionDto
            {
                FromWalletId = 1,
                ToWalletId = 2,
                Amount = 100
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _transactionService.CreateAsync(createTransactionDto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenInsufficientFunds()
        {
            // Arrange
            var fromWallet = new Wallet { Id = 1, Balance = 50 , DocumentId = "9999999999" , Name="Test1"};
            var toWallet = new Wallet { Id = 2, Balance = 0, DocumentId = "1111111111", Name = "Test2" };
            
            _context.Wallets.RemoveRange(_context.Wallets);
            _context.Transactions.RemoveRange(_context.Transactions);


            _context.Wallets.Add(fromWallet);
            _context.Wallets.Add(toWallet);

            await _context.SaveChangesAsync();

            var createTransactionDto = new CreateTransactionDto
            {
                FromWalletId = 1,
                ToWalletId = 2,
                Amount = 100
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _transactionService.CreateAsync(createTransactionDto));
            Assert.Equal("Fondos insuficientes", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateTransaction_WhenValid()
        {
            // Arrange
            var fromWallet = new Wallet { Id = 1, Balance = 500, DocumentId = "9999999999", Name = "Test1" };
            var toWallet = new Wallet { Id = 2, Balance = 0, DocumentId = "2222222222", Name = "Test2" };

            _context.Wallets.RemoveRange(_context.Wallets);
            _context.Transactions.RemoveRange(_context.Transactions);

            _context.Wallets.Add(fromWallet);
            _context.Wallets.Add(toWallet);
            await _context.SaveChangesAsync();

            var createTransactionDto = new CreateTransactionDto
            {
                FromWalletId = 1,
                ToWalletId = 2,
                Amount = 100
            };

            // Act
            var transactionDto = await _transactionService.CreateAsync(createTransactionDto);

            // Assert
            var transaction = await _context.Transactions.FindAsync(transactionDto.Id);
            Assert.NotNull(transaction);
            Assert.Equal(400, fromWallet.Balance); 
            Assert.Equal(100, toWallet.Balance);
        }

    }
    }
