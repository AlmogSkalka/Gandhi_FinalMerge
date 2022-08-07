using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ghandi_dev_3._0.Models.DAL;

namespace ghandi_dev_3._0.Models
{
    public class Color
    {
        int colorId;
        string colorDesc;

        public Color(int colorId, string colorDesc)
        {
            ColorId = colorId;
            ColorDesc = colorDesc;
        }

        public int ColorId { get => colorId; set => colorId = value; }
        public string ColorDesc { get => colorDesc; set => colorDesc = value; }

        public Color() { }
        
        public List<Color> ReadColors()
        {
            DataServices ds = new DataServices();
            List<Color> colors = ds.ReadColors();
            return colors;
        }
    }
}