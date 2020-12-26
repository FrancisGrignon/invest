using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Invest.MVC;

namespace Invest.MVC
{
    public class InvestContext : DbContext
    {
        public DbSet<Investor> Investors { get; set; }

        public DbSet<Investment> Investments { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Forex> Forexes { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<Transaction> Transactions { get; set; }        

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer(@"Server =.\SQLExpress; AttachDbFilename=C:\Temp\Invest.mdf;Database=dbname;Trusted_Connection=Yes;");
            options.UseSqlServer(@"Server = localhost; Database = invest; User Id = sa; Password = P@ssword66");

            options.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>()
                .HasIndex(p => new { p.Symbol })
                .IsUnique(true);

            var operations = new Operation[] {
                new Operation { OperationId = Operation.Buy, Name = "Buy", Created = DateTime.UtcNow },
                new Operation { OperationId = Operation.Dividend, Name = "Dividend", Created = DateTime.UtcNow },
                new Operation { OperationId = Operation.Merge, Name = "Merge", Created = DateTime.UtcNow },
                new Operation { OperationId = Operation.Sell, Name = "Sell", Created = DateTime.UtcNow },
                new Operation { OperationId = Operation.Split, Name = "Split", Created = DateTime.UtcNow }
            };

            modelBuilder.Entity<Operation>().HasData(operations);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Invest.MVC.StockHistory> StockHistory { get; set; }
    }

    public interface IEntity
    {
        DateTime Created { get; set; }
    }
      
    public class Investor : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestorId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress()]
        [MaxLength(255)]
        public string Email { get; set; }

        public DateTime Created { get; set; }

        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public List<Investment> Stocks { get; } = new List<Investment>();
    }

    public class Stock : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Symbol { get; set; }

        [Range(0, Int32.MaxValue)]
        public decimal Value { get; set; }

        // CAD, USD
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Currency { get; set; }

        public DateTime Created { get; set; }

        public List<StockHistory> StockHistories { get; } = new List<StockHistory>();
    }

    public class StockHistory : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockHistoryId { get; set; }

        public Stock Stock { get; set; }

        public int StockId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Symbol { get; set; }

        [Range(0, Int32.MaxValue)]
        public decimal Value { get; set; }

        // CAD, USD
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Currency { get; set; }

        public DateTime Created { get; set; }
    }

    public class Investment : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestmentId { get; set; }

        public int StockId { get; set; }

        public Stock Stock { get; set; }

        public int InvestorId { get; set; }

        public Investor Investor { get; set; }

        [Range(0, Int32.MaxValue)]
        public decimal Quantity { get; set; }

        public DateTime Created { get; set; }
    }

    public class Forex : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ForexId { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string ExchangeRate { get; set; }

        public DateTime Created { get; set; }
    }

    public class Transaction : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        public int StockId { get; set; }

        public Stock Stock { get; set; }

        public int OperationId { get; set; }

        public Operation Operation { get; set; }

        public int InvestorId { get; set; }

        public Investor Investor { get; set; }

        [Range(0, Int32.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Currency { get; set; }

        [Range(0, Int32.MaxValue)]
        public decimal Quantity { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public DateTime Created { get; set; }
    }

    public class Operation : IEntity
    {
        public const int Buy = 1;
        public const int Sell = 2;
        public const int Split = 3;
        public const int Merge = 4;
        public const int Dividend = 5;

        public int OperationId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public DateTime Created { get; set; }
    }

}