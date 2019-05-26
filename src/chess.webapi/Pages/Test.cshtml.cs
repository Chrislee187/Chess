using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace chess.webapi.Pages
{
    public class TestModel : PageModel
    {
        public IActionResult OnGet()
        {
            TestValue = "Flip";
            return Page();
        }

        public void OnPost()
        {
            if (TestValue == "Flip")
            {
                TestValue = "Flop";
            } else if (TestValue == "Flop")
            {
                TestValue = "Flip";
            } 
        }

        [BindProperty]
        public string TestValue { get; set; }
    }
}
