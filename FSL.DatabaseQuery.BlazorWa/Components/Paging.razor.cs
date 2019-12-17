using Microsoft.AspNetCore.Components;

namespace FSL.DatabaseQuery.BlazorWa.Components
{
    public class PagingComponent : ComponentBase
    {
        [Parameter]
        public int Page { get; set; }

        [Parameter]
        public int TotalRecords { get; set; }

        [Parameter]
        public int Rows { get; set; }

        [Parameter]
        public int Count { get; set; }

        [Parameter]
        public EventCallback<int> OnClickAsync { get; set; }
    }
}
