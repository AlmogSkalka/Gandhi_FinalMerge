using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ghandi_dev_3._0.Models.DAL;

namespace ghandi_dev_3._0.Models
{
    public class QandA
    {
        int Qid;
        string question;
        string answer;

        public QandA(string question, string answer)
        {
            this.Question = question;
            this.Answer = answer;
        }
        public QandA() { }

        public QandA(string question, int qid1)
        {
            Question = question;
            Qid1 = qid1;
        }

        public string Question { get => question; set => question = value; }
        public string Answer { get => answer; set => answer = value; }
        public int Qid1 { get => Qid; set => Qid = value; }

        public QandA ReadQandA (string Uemail)
        {
           DataServices ds = new DataServices();
           QandA QandA = ds.ReadQandA(Uemail);
           return QandA;
        }
        public List<QandA> GetAllQ()
        {
            DataServices ds = new DataServices();
            return ds.GetAllQ();
        }
    }
}