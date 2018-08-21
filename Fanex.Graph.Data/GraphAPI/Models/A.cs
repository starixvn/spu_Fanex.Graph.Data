using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphAPI.Models
{
    public class A
    {
        public int Id { get; set; }

        public O o { get; set; }
    }

    public class O
    {
        public int a { get; set; }

        public int b { get; set; }
    }
}