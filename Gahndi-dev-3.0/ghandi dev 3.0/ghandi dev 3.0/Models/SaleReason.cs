using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ghandi_dev_3._0.Models.DAL;

namespace ghandi_dev_3._0.Models
{
    public class SaleReason
    {
        int reasonId;
        string reasonDesc;

        public SaleReason() { }
        public SaleReason(string reasonDesc)
        {
            ReasonDesc = reasonDesc;
        }

        public int ReasonId { get => reasonId; set => reasonId = value; }
        public string ReasonDesc { get => reasonDesc; set => reasonDesc = value; }

        public List<SaleReason> ReadReasons()
        {
            DataServices ds = new DataServices();
            List<SaleReason> reasons = ds.ReadReasons();
            return reasons;
        }
    }
}