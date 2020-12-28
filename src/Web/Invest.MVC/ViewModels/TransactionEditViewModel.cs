using System;
using System.ComponentModel.DataAnnotations;

namespace Invest.MVC.ViewModels
{
    public class TransactionEditViewModel
    {
        public int TransactionId { get; set; }

        public int? StockId { get; set; }

        public int OperationId { get; set; }

        public int InvestorId { get; set; }

        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^\d+([\.,]\d{1,2})?$")]
        public string Amount { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Currency { get; set; }

        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^\d+([\.,]\d{1,2})?$")]
        public string Quantity { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    }
}
