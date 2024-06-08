#region Usings

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace NewmanRingsTwice.Domain.Models
{ 
    public class MailListGetRequest
    {
        public FetchFormRequest FormData { get; set; }
        public FetchSortRequest SortData { get; set; }
        public FetchPagingRequest PagingData { get; set; }
    }

    public class FetchPagingRequest
    {
        public const int ItemsPerPage = 10;

        public int NewPage { get; set; }

        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 0;

        public int PageNumTotal { get; set; } = 0;

        public void SetPagingParams(int numOfItems)
        {
            var pageNumTotal = numOfItems / ItemsPerPage;

            if (numOfItems % ItemsPerPage == 0)
            {
                PageNumTotal = pageNumTotal;
            }
            else
            {
                PageNumTotal = pageNumTotal + 1;
            }

            Skip = ItemsPerPage * (NewPage - 1);
            Take = ItemsPerPage;
        }
    }

    public class FetchSortRequest
    {
        public string ActiveColumn { get; set; } = "DatetimeCreated";
        public bool Ascending { get; set; } = true;
    }

    public class FetchFormRequest
    {
        public string Search { get; set; }
    } 

    public class MailListGetResponse
    {
        public int CurrentPage { get; set; } = 1;
        public int PageNumTotal { get; set; } = 1;

        public string ActiveColumn { get; set; } = "DatetimeCreated";
        public bool Ascending { get; set; } = true;

        public List<MailListItemModel> MailList { get; set; } = new List<MailListItemModel>();
        public bool HasItems => MailList.Any();
    }

    public class MailListItemModel
    {
        public int MailID { get; set; }
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public string Subject { get; set; }
        public int ImportanceID { get; set; }
        public ImportanceType Importance => (ImportanceType)ImportanceID;
        public DateTime DatetimeCreated { get; set; }
    }

    public class MailGetResponse
    {
        public MailReadModel MailInfo { get; set; }

        public MailGetResponse(MailReadModel mail)
        {
            MailInfo = mail;
        }
    }

    public class MailReadModel
    {
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public string CcMailListString { get; set; }
        public List<string> CcMailList { get; set; } = new List<string>();
        public string Subject { get; set; }
        public ImportanceType Importance => (ImportanceType)ImportanceID;
        public int ImportanceID { get; set; }
        public string MailContent { get; set; }

        public DateTime DatetimeCreated { get; set; }
    }    
}