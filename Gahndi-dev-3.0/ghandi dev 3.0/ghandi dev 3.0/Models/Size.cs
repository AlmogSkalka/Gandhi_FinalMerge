using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ghandi_dev_3._0.Models.DAL;

namespace ghandi_dev_3._0.Models
{
    public class Size
    {
        int sizeId;
        string sizeDesc;

        public Size(int sizeId, string sizeDesc)
        {
            SizeId = sizeId;
            SizeDesc = sizeDesc;
        }

        public int SizeId { get => sizeId; set => sizeId = value; }
        public string SizeDesc { get => sizeDesc; set => sizeDesc = value; }

        public Size() { }
        public List<Size> ReadSize() {
            DataServices ds = new DataServices();
            List<Size> sizes = ds.ReadSizes();
            return sizes;
        }
    }
}