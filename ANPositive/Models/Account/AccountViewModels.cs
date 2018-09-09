using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ANPositive.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Bu Bilgisayarda Beni Hatırla?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Eposta")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Geçerli bir eposta adresi girmeniz gerekiyor!")]
        [Display(Name = "Eposta")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parolanızı girmeniz gerekiyor!")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Geçerli bir eposta adresi girmeniz gerekiyor!")]
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Parolayı Doğrula")]
        [Compare("Password", ErrorMessage = "Parolalarınız birbiri ile uyuşmuyor.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(24, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 2)]
        [Display(Name = "Adı")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(24, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 2)]
        [Display(Name = "Soyadı")]
        public string LastName { get; set; }

        [Display(Name = "Görev Tanımı")]
        public string Title { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Geçerli bir eposta adresi girmeniz gerekiyor!")]
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Parolayı Doğrula")]
        [Compare("Password", ErrorMessage = "Parolalarınız birbiri ile uyuşmuyor.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Geçerli bir eposta adresi girmeniz gerekiyor!")]
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string Email { get; set; }
    }
}
