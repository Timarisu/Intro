using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace Intro.Models.ViewModels
{
    public class BlogVM
    {
        public Blog Blog { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}