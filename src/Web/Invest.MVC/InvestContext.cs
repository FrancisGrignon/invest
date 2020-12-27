using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Invest.MVC;
using System.ComponentModel;

namespace Invest.MVC
{
    public class InvestContext : DbContext
    {
        public DbSet<Investor> Investors { get; set; }

        public DbSet<Investment> Investments { get; set; }

        public DbSet<InvestmentHistory> InvestmentHistories { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<StockHistory> StockHistories { get; set; }

        public DbSet<Forex> Forexes { get; set; }

        public DbSet<ForexHistory> ForexHistories { get; set; }

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
            modelBuilder.Entity<Stock>().HasMany(c => c.StockHistories).WithOne(c => c.Stock).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Investment>().HasMany(c => c.InvestmentHistories).WithOne(c => c.Investment).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Forex>().HasMany(c => c.ForexHistories).WithOne(c => c.Forex).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Stock>()
                .HasIndex(p => new { p.Symbol })
                .IsUnique(true);

            modelBuilder.Entity<StockHistory>()
                .HasIndex(p => new { p.Symbol });

            var operations = new Operation[] {
                new Operation { OperationId = Operation.Buy, Name = "Buy" },
                new Operation { OperationId = Operation.Dividend, Name = "Dividend" },
                new Operation { OperationId = Operation.Merge, Name = "Merge" },
                new Operation { OperationId = Operation.Sell, Name = "Sell" },
                new Operation { OperationId = Operation.Split, Name = "Split" }
            };

            modelBuilder.Entity<Operation>().HasData(operations);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Invest.MVC.StockHistory> StockHistory { get; set; }
    }

    public interface IEntity
    {
        DateTime CreatedUtc { get; set; }

        DateTime UpdatedUtc { get; set; }

        bool Enable { get; set; }
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

        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public List<Investment> Investments { get; } = new List<Investment>();

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; }
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

        public List<StockHistory> StockHistories { get; } = new List<StockHistory>();

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;
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

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;

        public static StockHistory StockFrom(Stock stock)
        {
            var history = new StockHistory
            {
                StockId = stock.StockId,
                Stock = stock,
                Name = stock.Name,
                Symbol = stock.Symbol,
                Value = stock.Value,
                Currency = stock.Currency,
                CreatedUtc = stock.CreatedUtc,
                UpdatedUtc = stock.UpdatedUtc,
                Enable = stock.Enable
            };

            return history;
        }
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

        // CAD, USD
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Currency { get; set; }

        public List<InvestmentHistory> InvestmentHistories { get; } = new List<InvestmentHistory>();

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;
    }

    public class InvestmentHistory : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestmentHistoryId { get; set; }

        public int InvestmentId { get; set; }

        public Investment Investment { get; set; }

        public int StockId { get; set; }

        public Stock Stock { get; set; }

        public int InvestorId { get; set; }

        public Investor Investor { get; set; }

        [Range(0, Int32.MaxValue)]
        public decimal Quantity { get; set; }

        [Range(0, Int32.MaxValue)]
        public decimal Value { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string ExchangeRate { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;

        public static InvestmentHistory CreateFrom(Investment investment)
        {
            var history = new InvestmentHistory
            {
                InvestmentId = investment.InvestmentId,
                Investment = investment,
                StockId = investment.StockId,
                Stock = investment.Stock,
                InvestorId = investment.InvestorId,
                Investor = investment.Investor,
                Quantity = investment.Quantity,
                Currency = investment.Currency,
                CreatedUtc = investment.CreatedUtc,
                UpdatedUtc = investment.UpdatedUtc,
                Enable = investment.Enable
            };

            return history;
        }
    }

    public class Forex : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ForexId { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string ExchangeRate { get; set; }

        public List<ForexHistory> ForexHistories { get; } = new List<ForexHistory>();

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;
    }

    public class ForexHistory : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ForexHistoryId { get; set; }

        public int ForexId { get; set; }

        public Forex Forex { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string ExchangeRate { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;

        public static ForexHistory CreateFrom(Forex forex)
        {
            var history = new ForexHistory
            {
                ForexId = forex.ForexId,
                Forex = forex,
                Currency = forex.Currency,
                ExchangeRate = forex.ExchangeRate,
                CreatedUtc = forex.CreatedUtc,
                UpdatedUtc = forex.UpdatedUtc,
                Enable = forex.Enable
            };

            return history;
        }
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
        public decimal Quantity { get; set; }

        [Range(0, Int32.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Currency { get; set; }

        [Required]
        public string ExchangeRate { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;
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

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;
    }

}