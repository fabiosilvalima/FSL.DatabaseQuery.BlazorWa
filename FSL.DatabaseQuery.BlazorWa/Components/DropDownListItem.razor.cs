using Microsoft.AspNetCore.Components;

namespace FSL.DatabaseQuery.BlazorWa.Components
{
    public class DropDownListItemComponent : ComponentBase
    {
        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public bool Selected { get; set; }

        [CascadingParameter]
        public DropDownListComponent ParentDropDownList { get; set; }

        protected override void OnInitialized()
        {
            ParentDropDownList?.AddItem(this);

            base.OnInitialized();
        }
    }
}
