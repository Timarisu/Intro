using System.Collections;
using System.Collections.Generic;

namespace Intro.Models.ViewModels
{
    public class HomeVM
    {
        public  IEnumerable <Blog> Blogs { get; set; }
        public IEnumerable <Category> Categories { get; set; }
    }
}
