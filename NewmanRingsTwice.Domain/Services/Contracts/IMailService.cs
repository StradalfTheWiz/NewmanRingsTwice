using NewmanRingsTwice.Domain.Models;

namespace NewmanRingsTwice.Domain.Services.Contracts
{
    public interface IMailService
    {
        MailGetResponse GetMail(int id);
        MailListGetResponse GetMailList(MailListGetRequest request);
        MailPostResponse PostMail(MailPostRequest request);
    }
}