using System.ComponentModel.DataAnnotations;

namespace Invest.MVC.ViewModels
{
    public class LoginViewModel
    {
        #region Properties  

        /// <summary>  
        /// Gets or sets to username address.  
        /// </summary>  
        [Required]
        [Display(Name = "Adresse courriel ou téléphone")]
        public string Username { get; set; }

        /// <summary>  
        /// Gets or sets to password address.  
        /// </summary>  
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        #endregion
    }
}
