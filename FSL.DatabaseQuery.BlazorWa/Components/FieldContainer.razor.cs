using Microsoft.AspNetCore.Components;

namespace FSL.DatabaseQuery.BlazorWa.Components
{
    public class FieldContainerComponent : ComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int Cols { get; set; }
    }
}
