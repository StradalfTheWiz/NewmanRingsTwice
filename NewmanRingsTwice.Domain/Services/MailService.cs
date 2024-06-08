#region Usings

using NewmanRingsTwice.Domain.Contracts.Repository;
using NewmanRingsTwice.Domain.Models;
using NewmanRingsTwice.Domain.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace NewmanRingsTwice.Domain.Services
{
    public class MailService : IMailService
    {
        private readonly IMailRepository _mailRepository;

        public MailService(IMailRepository mailRepository)
        {
            _mailRepository = mailRepository;
        }

        public MailPostResponse PostMail(MailPostRequest request)
        {
            var submit = new MailSubmitModel(request);

            if (submit.IsRequestValid())
            {
                submit.Response.CheckTrial(_mailRepository.GetNumOfMailsSaved());

                return _mailRepository.PostMail(submit);
            }
            else
            {
                submit.Response.IsSuccess = false;
                submit.Response.Message = "Invalid request.";

                return submit.Response;
            }
        }

        public MailListGetResponse GetMailList(MailListGetRequest request)
        {
            return _mailRepository.GetMailList(request);
        }

        public MailGetResponse GetMail(int id)
        {
            return _mailRepository.GetMail(id);
        }
    }
}