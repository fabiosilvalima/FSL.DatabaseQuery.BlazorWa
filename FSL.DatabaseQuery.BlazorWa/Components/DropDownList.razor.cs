using FSL.DatabaseQuery.Core.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSL.DatabaseQuery.BlazorWa.Components
{
    public class DropDownListComponent : ControlComponent
    {
        public DropDownListComponent()
        {
            DataSource = new List<IDropDownListItem>();
        }

        internal void AddItem(
            DropDownListItemComponent item)
        {
            DataSource.Add(new Item(item.Text, item.Value));

            if (item.Selected)
            {
                SelectedValue = item.Value ?? item.Text;
                SelectedValueChanged.InvokeAsync(SelectedValue).GetAwaiter();
            }
        }

        [Parameter]
        public string SelectedValue { get; set; }

        [Parameter]
        public EventCallback<string> SelectedValueChanged { get; set; }

        [Parameter]
        public EventCallback<string> OnValueChanged { get; set; }

        [Parameter]
        public List<IDropDownListItem> DataSource { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected async Task OnChangeAsync(
            ChangeEventArgs e)
        {
            SelectedValue = e.Value.ToString();

            await SelectedValueChanged.InvokeAsync(SelectedValue);
            await OnValueChanged.InvokeAsync(SelectedValue);
        }
    }
}
