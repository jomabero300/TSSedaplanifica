using TSSedaplanifica.Common;

namespace TSSedaplanifica.Helpers.Gene
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body, MemoryStream attachment = null);
    }
}
