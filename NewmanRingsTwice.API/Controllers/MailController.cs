using Microsoft.AspNetCore.Mvc;
using NewmanRingsTwice.Domain.Models;
using NewmanRingsTwice.Domain.Services.Contracts;

namespace NewmanRingsTwice.API.Controllers
{
    [Route("api/mail")]
    [ApiController]
    public class MailController : Controller
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("post-mail")]
        public MailPostResponse PostMail(MailPostRequest request)
        {
            return _mailService.PostMail(request);
        }

        [HttpPost("get-list")]
        public MailListGetResponse GetMailList(MailListGetRequest request)
        {
            return _mailService.GetMailList(request);   
        }

        [HttpGet("{id}")]
        public MailGetResponse GetMail(int id)
        {
            return _mailService.GetMail(id);
        }
    }
}
