﻿// <auto-generated />
using System;
using Invest.MVC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Invest.MVC.Migrations
{
    [DbContext(typeof(InvestContext))]
    partial class InvestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Invest.MVC.Forex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Currency");

                    b.ToTable("Forexes");
                });

            modelBuilder.Entity("Invest.MVC.ForexHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<DateTime>("DateUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ForexId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ForexId");

                    b.ToTable("ForexHistories");
                });

            modelBuilder.Entity("Invest.MVC.Investment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.Property<int>("InvestorId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(18, 5)
                        .HasColumnType("decimal(18,5)");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("InvestorId");

                    b.HasIndex("StockId");

                    b.ToTable("Investments");
                });

            modelBuilder.Entity("Invest.MVC.InvestmentHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<DateTime>("DateUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.Property<decimal>("ExchangeRate")
                        .HasPrecision(18, 5)
                        .HasColumnType("decimal(18,5)");

                    b.Property<int>("InvestmentId")
                        .HasColumnType("int");

                    b.Property<int>("InvestorId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(18, 5)
                        .HasColumnType("decimal(18,5)");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("InvestmentId");

                    b.HasIndex("InvestorId");

                    b.HasIndex("StockId");

                    b.ToTable("InvestmentHistories");
                });

            modelBuilder.Entity("Invest.MVC.Investor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Investors");
                });

            modelBuilder.Entity("Invest.MVC.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Operations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6405),
                            Enable = true,
                            Name = "Buy",
                            UpdatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6416)
                        },
                        new
                        {
                            Id = 5,
                            CreatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6845),
                            Enable = true,
                            Name = "Dividend",
                            UpdatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6847)
                        },
                        new
                        {
                            Id = 4,
                            CreatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6849),
                            Enable = true,
                            Name = "Merge",
                            UpdatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6850)
                        },
                        new
                        {
                            Id = 2,
                            CreatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6852),
                            Enable = true,
                            Name = "Sell",
                            UpdatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6853)
                        },
                        new
                        {
                            Id = 3,
                            CreatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6854),
                            Enable = true,
                            Name = "Split",
                            UpdatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6856)
                        },
                        new
                        {
                            Id = 6,
                            CreatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6857),
                            Enable = true,
                            Name = "Deposit",
                            UpdatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6858)
                        },
                        new
                        {
                            Id = 7,
                            CreatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6860),
                            Enable = true,
                            Name = "Withdraw",
                            UpdatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6861)
                        },
                        new
                        {
                            Id = 8,
                            CreatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6862),
                            Enable = true,
                            Name = "Transfer",
                            UpdatedUtc = new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6864)
                        });
                });

            modelBuilder.Entity("Invest.MVC.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("Symbol")
                        .IsUnique();

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("Invest.MVC.StockHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<DateTime>("DateUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("StockId");

                    b.HasIndex("Symbol");

                    b.ToTable("StockHistories");
                });

            modelBuilder.Entity("Invest.MVC.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<DateTime>("DateUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("InvestorId")
                        .HasColumnType("int");

                    b.Property<int>("OperationId")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("StockId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("InvestorId");

                    b.HasIndex("OperationId");

                    b.HasIndex("StockId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Invest.MVC.ForexHistory", b =>
                {
                    b.HasOne("Invest.MVC.Forex", "Forex")
                        .WithMany("ForexHistories")
                        .HasForeignKey("ForexId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Forex");
                });

            modelBuilder.Entity("Invest.MVC.Investment", b =>
                {
                    b.HasOne("Invest.MVC.Investor", "Investor")
                        .WithMany("Investments")
                        .HasForeignKey("InvestorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Invest.MVC.Stock", "Stock")
                        .WithMany()
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Investor");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("Invest.MVC.InvestmentHistory", b =>
                {
                    b.HasOne("Invest.MVC.Investment", "Investment")
                        .WithMany("InvestmentHistories")
                        .HasForeignKey("InvestmentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Invest.MVC.Investor", "Investor")
                        .WithMany()
                        .HasForeignKey("InvestorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Invest.MVC.Stock", "Stock")
                        .WithMany()
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Investment");

                    b.Navigation("Investor");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("Invest.MVC.StockHistory", b =>
                {
                    b.HasOne("Invest.MVC.Stock", "Stock")
                        .WithMany("StockHistories")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("Invest.MVC.Transaction", b =>
                {
                    b.HasOne("Invest.MVC.Investor", "Investor")
                        .WithMany("Transactions")
                        .HasForeignKey("InvestorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Invest.MVC.Operation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Invest.MVC.Stock", "Stock")
                        .WithMany()
                        .HasForeignKey("StockId");

                    b.Navigation("Investor");

                    b.Navigation("Operation");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("Invest.MVC.Forex", b =>
                {
                    b.Navigation("ForexHistories");
                });

            modelBuilder.Entity("Invest.MVC.Investment", b =>
                {
                    b.Navigation("InvestmentHistories");
                });

            modelBuilder.Entity("Invest.MVC.Investor", b =>
                {
                    b.Navigation("Investments");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Invest.MVC.Stock", b =>
                {
                    b.Navigation("StockHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
