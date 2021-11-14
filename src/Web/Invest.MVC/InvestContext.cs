using Invest.MVC.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invest.MVC
{
    public class InvestContext : DbContext
    {
        public DbSet<Forex> Forexes { get; set; }

        public DbSet<ForexHistory> ForexHistories { get; set; }

        public DbSet<Investor> Investors { get; set; }

        public DbSet<Investment> Investments { get; set; }

        public DbSet<InvestmentHistory> InvestmentHistories { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<StockHistory> StockHistories { get; set; }

        public DbSet<Transaction> Transactions { get; set; }        

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            ///options.UseSqlServer(@"Server = localhost,11433; Database = invest; User Id = sa; Password = P@ssword66");
            options.UseSqlite("Filename=invest.db");
            options.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>()
                .HasMany(c => c.StockHistories)
                .WithOne(c => c.Stock)
                .OnDelete(DeleteBehavior.NoAction);
          
            modelBuilder.Entity<Investment>()
                .HasMany(c => c.InvestmentHistories)
                .WithOne(c => c.Investment)
                .OnDelete(DeleteBehavior.NoAction);
           
            modelBuilder.Entity<Forex>()
                .HasMany(c => c.ForexHistories)
                .WithOne(c => c.Forex)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Stock>()
                .HasIndex(p => new { p.Symbol })
                .IsUnique(true);

            modelBuilder.Entity<StockHistory>()
                .HasIndex(p => new { p.Symbol });

            modelBuilder.Entity<Forex>()
                .HasIndex(p => new { p.Currency });

            modelBuilder.Entity<Investment>().Property(m => m.Quantity).HasPrecision(18, 5);
            modelBuilder.Entity<InvestmentHistory>().Property(m => m.Quantity).HasPrecision(18, 5);
            modelBuilder.Entity<InvestmentHistory>().Property(m => m.ExchangeRate).HasPrecision(18, 5);

            var operations = new Operation[] {
                new Operation { Id = Operation.Buy, Name = "Buy" },
                new Operation { Id = Operation.Dividend, Name = "Dividend" },
                new Operation { Id = Operation.Merge, Name = "Merge" },
                new Operation { Id = Operation.Sell, Name = "Sell" },
                new Operation { Id = Operation.Split, Name = "Split" },
                new Operation { Id = Operation.Deposit, Name = "Deposit" },
                new Operation { Id = Operation.Withdraw, Name = "Withdraw" },
                new Operation { Id = Operation.Transfer, Name = "Transfer" }
            };

            modelBuilder.Entity<Operation>().HasData(operations);

            base.OnModelCreating(modelBuilder);
        }

        public void Import()
        {
            var _importService = new ImportService(this);

            _importService.Execute();
        }
    }

    public interface IEntity
    {
        public int Id { get; set; }

        DateTime CreatedUtc { get; set; }

        DateTime UpdatedUtc { get; set; }

        bool Enable { get; set; }
    }

    public class Investor : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [EmailAddress()]
        [StringLength(255)]
        public string Email { get; set; }

        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public List<Investment> Investments { get; } = new List<Investment>();

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; }
    }

    public class Stock : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Symbol { get; set; }

        [Range(0, Int32.MaxValue)]
        public float Value { get; set; }

        // CAD, USD
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        public List<StockHistory> StockHistories { get; } = new List<StockHistory>();

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;
    }

    public class StockHistory : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public Stock Stock { get; set; }

        public int StockId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Symbol { get; set; }

        [Range(0, Int32.MaxValue)]
        public float Value { get; set; }

        // CAD, USD
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        public DateTime DateUtc { get; set; } = DateTime.UtcNow.Date;

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;

        public static StockHistory StockFrom(Stock stock)
        {
            var history = new StockHistory
            {
                StockId = stock.Id,
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StockId { get; set; }

        public Stock Stock { get; set; }

        public int InvestorId { get; set; }

        public Investor Investor { get; set; }

        [Range(0, Int32.MaxValue)]
        public float Quantity { get; set; }

        // CAD, USD
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        public List<InvestmentHistory> InvestmentHistories { get; } = new List<InvestmentHistory>();

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;
    }

    public class InvestmentHistory : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int InvestmentId { get; set; }

        public Investment Investment { get; set; }

        public int StockId { get; set; }

        public Stock Stock { get; set; }

        public int InvestorId { get; set; }

        public Investor Investor { get; set; }

        [Range(0, Int32.MaxValue)]
        public float Quantity { get; set; }

        [Range(0, Int32.MaxValue)]
        public float Value { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        public float ExchangeRate { get; set; }

        public DateTime DateUtc { get; set; } = DateTime.UtcNow.Date;

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;
    }

    public class Forex : IEntity
    {
        public const string CAD = "CAD";
        public const string USD = "USD";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        public float ExchangeRate { get; set; }

        public List<ForexHistory> ForexHistories { get; } = new List<ForexHistory>();

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;
    }

    public class ForexHistory : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ForexId { get; set; }

        public Forex Forex { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        public float ExchangeRate { get; set; }

        public DateTime DateUtc { get; set; } = DateTime.UtcNow.Date;

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;

        public static ForexHistory CreateFrom(Forex forex)
        {
            var history = new ForexHistory
            {
                ForexId = forex.Id,
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? StockId { get; set; }

        public Stock Stock { get; set; }

        public int OperationId { get; set; }

        public Operation Operation { get; set; }

        public int InvestorId { get; set; }

        public Investor Investor { get; set; }

        [Range(0, Int32.MaxValue)]
        public float Quantity { get; set; }

        [Range(0, Int32.MaxValue)]
        public float Amount { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        public float ExchangeRate { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime DateUtc { get; set; } = DateTime.UtcNow.Date;

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
        public const int Deposit = 6;
        public const int Withdraw = 7;
        public const int Transfer = 8;

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

        public bool Enable { get; set; } = true;
    }
}