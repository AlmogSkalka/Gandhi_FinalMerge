using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ghandi_dev_3._0.Models.DAL; 


namespace ghandi_dev_3._0.Models
{
    public class Brand
    {
        int brandId;
        string brandDesc;
        string brandUrl;

        public Brand(int brandId, string brandDesc, string brandUrl)
        {
            BrandId = brandId;
            BrandDesc = brandDesc;
            BrandUrl = brandUrl;
        }

        public int BrandId { get => brandId; set => brandId = value; }
        public string BrandDesc { get => brandDesc; set => brandDesc = value; }
        public string BrandUrl { get => brandUrl; set => brandUrl = value; }

        public Brand() { }
        public List<Brand> ReadBrands() {

            DataServices ds = new DataServices();
            List<Brand> brands = ds.ReadBrands();
            return brands;
        }
    }
}