using Microsoft.AspNetCore.Components;
using PgnReader;

namespace chess.blazor.Pages
{
    // NOTE: As of Core 3.0.0 preview6 blazor doesn't use partial classes for this so we have
    // to create a base class the inherits from component base and is then inherited by the .razor 
    // file.
    // We then need to use Property Injection to inject required services.
    public class ConvertBlazorModel : ComponentBase
    {
        [Inject]
        public IPgnSerialisationService PgnSerialisationService { get; set; }

        protected PgnConversionModel Model = new PgnConversionModel();

        protected void Convert()
        {
            // C# in the browser, who'da thought!
            Model.PgJson = PgnSerialisationService.SerializeAllGames(Model.PgnText, Model.ExpandedFormat);
        }
    }
}