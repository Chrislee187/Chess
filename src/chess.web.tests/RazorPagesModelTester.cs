using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace chess.web.tests
{
    public class RazorPagesModelTester
    {
        private readonly PageModel _model;

        public RazorPagesModelTester(PageModel model)
        {
            _model = model;
        }
        public RazorPagesModelTester WithInvalidModelState(string fieldKey = "AnyField", string message = "Is in invalid")
        {
            var modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.AddModelError(fieldKey, message);
            _model.PageContext = new PageContext(new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new PageActionDescriptor(),
                modelStateDictionary
            ));

            return this;
        }
    }
}