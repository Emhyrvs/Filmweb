using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Filmweb.Models
{
    public class Pomoc
    {
        public List<Actor> actors { get; set; }
        public Movie movie { get; set; }
        public Series series { get; set; }
        public int MyProperty { get; set; }
    }
}