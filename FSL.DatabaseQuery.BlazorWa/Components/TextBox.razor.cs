using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FSL.DatabaseQuery.BlazorWa.Components
{
    public class TextBoxComponent : ControlComponent
    {
        public TextBoxComponent()
        {
            MaxLength = "500";
        }

        [Parameter]
        public bool Required { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public string MaxLength { get; set; }

        [Parameter]
        public string Rows { get; set; }

        [Parameter]
        public TextBoxMode TextMode { get; set; }

        [Parameter]
        public EventCallback<string> TextChanged { get; set; }

        [Parameter]
        public EventCallback<string> MaxLengthChanged { get; set; }

        [Parameter]
        public EventCallback<string> TextValueChanged { get; set; }

        protected async Task OnChangeAsync(
            ChangeEventArgs e)
        {
            Text = e.Value as string;
            IsValid = !(Required && string.IsNullOrEmpty(Text));

            await TextChanged.InvokeAsync(Text);
            await TextValueChanged.InvokeAsync(Text);
        }
    }

    public enum TextBoxMode
    {
        SingleLine = 0,
        MultiLine = 1,
        Password = 2,
        Number = 3
    }
}
