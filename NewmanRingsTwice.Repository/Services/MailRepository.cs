#region Usings

using Microsoft.EntityFrameworkCore;
using NewmanRingsTwice.DB.NewmanDB;
using NewmanRingsTwice.Domain.Contracts.Repository;
using NewmanRingsTwice.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace NewmanRingsTwice.Repository.Services
{
    public class MailRepository : IMailRepository
    {
        private readonly NewmanDBContext _db;

        public MailRepository(NewmanDBContext db)
        {
            _db = db;
        }

        public int GetNumOfMailsSaved()
        {
            return _db.Mail.Count();
        }

        public MailPostResponse PostMail(MailSubmitModel submit)
        {
            if (!submit.Response.IsTrialOver)
            {
                var newMail = new Mail()
                {
                    FromMail = submit.Request.FromMail,
                    ToMail = submit.Request.ToMail,
                    Subject = submit.Request.Subject,
                    MailContent = submit.Request.MailContent,
                    DatetimeCreated = submit.DatetimeCreated,
                    ImportanceTypeId = (int)submit.Request.Importance
                };

                _db.Mail.Add(newMail);

                foreach (var cc in submit.Request.CcMailList)
                {
                    var ccNew = new CcMailAddress()
                    {
                        Address = cc,
                        Mail = newMail
                    };

                    _db.CcMailAddresses.Add(ccNew);
                }

                _db.SaveChanges(); 
            }

            return submit.Response;
        }

        public MailListGetResponse GetMailList(MailListGetRequest request)
        {
            var result = new MailListGetResponse();

            var mailEntityListQuery = _db.Mail.AsQueryable();

            // Search.
            if (request.FormData != null)
            {
                if (!string.IsNullOrWhiteSpace(request.FormData.Search))
                {
                    mailEntityListQuery = mailEntityListQuery
                        .Where(x => x.Subject.Contains(request.FormData.Search)
                                || x.FromMail.Contains(request.FormData.Search)
                                || x.ToMail.Contains(request.FormData.Search)
                                || x.MailContent.Contains(request.FormData.Search));
                }
            }

            // Sort.
            if (request.SortData.ActiveColumn != null)
            {
                if (request.SortData.ActiveColumn == "DatetimeCreated")
                {
                    if (request.SortData.Ascending)
                    {
                        mailEntityListQuery = mailEntityListQuery.OrderBy(x => x.DatetimeCreated);
                        result.Ascending = true;
                    }
                    else
                    {
                        mailEntityListQuery = mailEntityListQuery.OrderByDescending(x => x.DatetimeCreated);
                        result.Ascending = false;
                    }
                }
                else if (request.SortData.ActiveColumn == "Importance")
                {
                    if (request.SortData.Ascending)
                    {
                        mailEntityListQuery = mailEntityListQuery.OrderBy(x => x.ImportanceTypeId);
                        result.Ascending = true;
                    }
                    else
                    {
                        mailEntityListQuery = mailEntityListQuery.OrderByDescending(x => x.ImportanceTypeId);
                        result.Ascending = false;
                    }
                }

                result.ActiveColumn = request.SortData.ActiveColumn;
            }
            else
            {
                // Default.
                mailEntityListQuery = mailEntityListQuery.OrderBy(x => x.DatetimeCreated);
            }

            // Pagination.
            var numOfMails = mailEntityListQuery.Count();

            if (request.PagingData != null)
            {
                request.PagingData.SetPagingParams(numOfMails);
                mailEntityListQuery = mailEntityListQuery.Skip(request.PagingData.Skip).Take(request.PagingData.Take);

                result.CurrentPage = request.PagingData.NewPage;
                result.PageNumTotal = request.PagingData.PageNumTotal;
            }

            CreateMailList(mailEntityListQuery, result);

            return result;
        }

        private void CreateMailList(IQueryable<Mail> mailEntityListQuery, MailListGetResponse result)
        {
            foreach (var mailEntity in mailEntityListQuery)
            {
                var mailSingle = new MailListItemModel()
                {
                    MailID = mailEntity.MailId,
                    FromMail = mailEntity.FromMail,
                    ToMail = mailEntity.ToMail,
                    Subject = mailEntity.Subject,
                    ImportanceID = mailEntity.ImportanceTypeId,
                    DatetimeCreated = mailEntity.DatetimeCreated
                };

                result.MailList.Add(mailSingle);
            }
        }

        public MailGetResponse GetMail(int id)
        {
            var mailEntity = _db.Mail
                .Include(x => x.CcMailAddresses)
                .FirstOrDefault(x => x.MailId == id);

            if (mailEntity != null)
            {
                var mail = new MailReadModel();

                mail.FromMail = mailEntity.FromMail;
                mail.ToMail = mailEntity.ToMail;
                mail.Subject = mailEntity.Subject;
                mail.ImportanceID = mailEntity.ImportanceTypeId;
                mail.MailContent = mailEntity.MailContent;
                mail.DatetimeCreated = mailEntity.DatetimeCreated;

                if (mailEntity.CcMailAddresses.Any())
                {
                    var list = mailEntity.CcMailAddresses.Select(x => x.Address).ToList();
                    mail.CcMailListString = string.Join("; ", list);
                    list.ForEach(x => mail.CcMailList.Add(x));
                }

                return new MailGetResponse(mail);
            }

            return null;
        }
    }
}