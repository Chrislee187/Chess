using Microsoft.AspNetCore.Components;


namespace chess.blazor.Components
{
    /// <summary>
    /// Simple component to generate an anchor tags that targets a new tab
    /// </summary>
    public class ExternalAComponent : ComponentBase
    {
        [Parameter]
        public string href { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}