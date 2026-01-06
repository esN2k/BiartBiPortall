namespace BiartBiPortal.Models
{
    /// <summary>
    /// Rapor ve Kategori arasındaki çoka-çok ilişki için ara tablo
    /// </summary>
    public class ReportCategory
    {
        public int ReportId { get; set; }
        public Report Report { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
