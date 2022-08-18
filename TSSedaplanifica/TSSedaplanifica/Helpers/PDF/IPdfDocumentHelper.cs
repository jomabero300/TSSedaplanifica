namespace TSSedaplanifica.Helpers.PDF
{
    public interface IPdfDocumentHelper
    {
        Task<MemoryStream> ReportListAsync(string title);
        Task<MemoryStream> ReportSoliAsync(int id);
        Task<MemoryStream> ReportSchoolAsync(int id);
    }
}
