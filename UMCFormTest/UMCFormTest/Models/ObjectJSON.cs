using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UMCFormTest.Models
{
    public class Answer
    {
        public int index { get; set; }
        public string value { get; set; }
        public string point { get; set; }
        public string comment { get; set; }
    }
    public class Question
    {
        public int index { get; set; }
        public string vi { get; set; }
        public string ja { get; set; }
    }
}