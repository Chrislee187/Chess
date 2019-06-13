using Microsoft.AspNetCore.Components;
using PgnReader;

namespace chess.blazor.Pages.Pgn.Conversion
{
    // NOTE: As of Core 3.0.0 preview6 blazor doesn't use partial classes for this so we have
    // to create a base class the inherits from component base and is then inherited by the .razor 
    // file if we want to avoid putting this code in .razor files
    // We then use Property Injection to handle dependencies.
    public class ConvertComponent : ComponentBase
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