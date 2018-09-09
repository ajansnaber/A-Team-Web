using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

namespace ANPositive.Models
{
    [Table("Galleries")]
    public class Gallery
    {
        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("Title")]
        [Required]
        [StringLength(140, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 2)]
        [Display(Name = "Başlık")]
        public string title { get; set; }

        [Column("Images")]
        [Required]
        [Display(Name = "Görseller")]
        public string tempImages
        {
            get => images != null ? string.Join(",", images) : null;
            set => images = value?.Split(',').ToList();
        }

        /// <summary>
        ///     Hack for Images List String
        /// </summary>
        public ICollection<string> images { get; set; }

        [Column("MetaTitle")]
        [StringLength(65, ErrorMessage = "{0} en fazla {1} karakter uzunluğunda olmalıdır.")]
        [Display(Name = "Meta Başlığı")]
        public string metaTitle { get; set; }

        [Column("MetaDescription")]
        [StringLength(320, ErrorMessage = "{0} en fazla {1} karakter uzunluğunda olmalıdır.")]
        [Display(Name = "Meta Açıklaması")]
        public string metaDescription { get; set; }

        [Column("MetaTags")]
        [Display(Name = "Meta Etiketleri")]
        public string tags
        {
            get => metaTags != null ? string.Join(",", metaTags) : null;
            set => metaTags = value?.Split(',').ToList();
        }

        /// <summary>
        ///     Hack for Metatags List String
        /// </summary>
        public ICollection<string> metaTags { get; set; }

        [Column("Language")] public int language { get; set; }

        [Column("Published")] public bool published { get; set; }

        [Column("DisplayOrder")] public int displayOrder { get; set; }
        
        [Column("CreatedAt")]
        public DateTime? createdAt { get; set; }

        [Column("ModifiedBy")]
        [MaxLength(128)]
        public string modifiedBy { get; set; }

        [Column("ModifiedAt")] public DateTime? modifiedAt { get; set; }

        [Column("IsDeleted")] public bool isDeleted { get; set; }

        public virtual string seoUrl()
        {
            string phrase = $"{id}-{title}";

            string str = RemoveAccent(phrase).ToLower();
            str = Regex.Replace(str, "ı", "i");
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = Regex.Replace(str, @"\s", "-");
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}