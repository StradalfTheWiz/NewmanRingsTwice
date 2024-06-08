using NewmanRingsTwice.Domain.Models;

namespace NewmanRingsTwice.Domain.Contracts.Repository
{
    public interface IMailRepository
    {
        MailGetResponse GetMail(int id);
        MailListGetResponse GetMailList(MailListGetRequest request);
        int GetNumOfMailsSaved();
        MailPostResponse PostMail(MailSubmitModel submit);
    }
}