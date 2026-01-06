using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BiartBiPortal.Models
{
    public class Report
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Rapor başlığı gereklidir.")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rapor içeriği gereklidir.")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Özet en fazla 1000 karakter olabilir.")]
        [Display(Name = "Özet")]
        public string? Summary { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Yayınlanma Tarihi")]
        public DateTime? PublishedDate { get; set; }

        [Display(Name = "Yayında")]
        public bool IsPublished { get; set; } = false;

        // Oluşturan kullanıcının ID'si
        [Display(Name = "Oluşturan Kullanıcı")]
        public string? CreatedByUserId { get; set; }

        // İlişkili nesneler
        public IdentityUser? CreatedByUser { get; set; }
        public ICollection<ReportCategory> ReportCategories { get; set; } = new List<ReportCategory>();
    }
}
