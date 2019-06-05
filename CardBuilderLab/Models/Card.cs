using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardBuilderLab.Models
{
    public class Card
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Suit { get; set; }
        public string Code { get; set; }
    }
}