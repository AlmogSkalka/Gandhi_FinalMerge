using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ghandi_dev_3._0.Models.DAL;

namespace ghandi_dev_3._0.Models
{
    public class Tag
    {
        int tagId;
        string tagDesc;
        int tagScore;

        public Tag(int tagId, string tagDesc, int tagScore)
        {
            TagId = tagId;
            TagDesc = tagDesc;
            TagScore = tagScore;


        }

        public int TagId { get => tagId; set => tagId = value; }
        public string TagDesc { get => tagDesc; set => tagDesc = value; }
        public int TagScore { get => tagScore; set => tagScore = value; }

        public Tag() { }

        public List<Tag> ReadTags() {
            DataServices ds = new DataServices();
            List<Tag> tags = ds.ReadTags();
            return tags;
        }

        public int InsertNewTag()
        {
            DataServices ds = new DataServices();
            return ds.InsertNewTag(this);
        }

    }
}