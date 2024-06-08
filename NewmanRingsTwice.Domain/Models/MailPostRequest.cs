#region Usings

using RegexExamples;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace NewmanRingsTwice.Domain.Models
{
    public class MailPostRequest
    {
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public List<string> CcMailList { get; set; } = new List<string>();
        public string Subject { get; set; }
        public ImportanceType Importance { get; set; } = ImportanceType.Unknown;
        public string MailContent { get; set; }
    }

    public class MailPostResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }

        public bool IsTrialOver { get; private set; } = false;

        public void CheckTrial(int numOfMailsSaved)
        {
            IsTrialOver = numOfMailsSaved >= 100;
        }
    }

    public class MailSubmitModel
    {
        public DateTime DatetimeCreated => DateTime.UtcNow;
        public MailPostRequest Request { get; }

        public MailPostResponse Response { get; set; } = new MailPostResponse();

        public bool IsRequestValid()
        {
            return HasRequired() && IsLengthValid() && IsValidMail();
        }

        private bool HasRequired()
        {
            return !(string.IsNullOrWhiteSpace(Request.FromMail)
                || string.IsNullOrWhiteSpace(Request.ToMail)
                || string.IsNullOrWhiteSpace(Request.Subject));
        }

        private bool IsLengthValid()
        {
            var mailLengthMax = 50;
            var subjectLength = 100;
            var contentLengthMax = 1000;

            return !(Request.FromMail?.Length > mailLengthMax
                 || Request.ToMail?.Length > mailLengthMax
                 || Request.Subject?.Length > subjectLength
                 || Request.MailContent?.Length > contentLengthMax
                 || Request.CcMailList.Any(x => x.Length > mailLengthMax));
        }

        private bool IsValidMail()
        {
            return (RegexUtilities.IsValidEmail(Request.FromMail)
                || RegexUtilities.IsValidEmail(Request.ToMail)
                || (Request.CcMailList.Any() && Request.CcMailList.All(x => RegexUtilities.IsValidEmail(x))));
        }

        public MailSubmitModel(MailPostRequest request)
        {
            Request = request;
        }
    }

    public enum ImportanceType
    {
        Low = 1,
        Normal = 2,
        High = 3,
        Unknown = 0
    }
}