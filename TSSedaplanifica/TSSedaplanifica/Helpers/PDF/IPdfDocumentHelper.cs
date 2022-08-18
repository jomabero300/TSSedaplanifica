namespace TSSedaplanifica.Helpers.PDF
{
    public interface IPdfDocumentHelper
    {
        Task<MemoryStream> ReportAsync(string title);
    }
}
