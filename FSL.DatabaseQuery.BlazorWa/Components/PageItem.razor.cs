using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace FSL.DatabaseQuery.BlazorWa.Components
{
    public class PageItemComponent : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public bool IsTrue { get; set; }

        [Parameter]
        public int RequestedPage { get; set; }

        [Parameter]
        public EventCallback<int> OnClickAsync { get; set; }

        protected async Task OnPagingAsync(
            EventArgs e)
        {
            await OnClickAsync.InvokeAsync(RequestedPage);
        }

        protected string IsDisabled(
            bool isOk)
        {
            return !isOk ? "disabled" : "";
        }
    }
}
