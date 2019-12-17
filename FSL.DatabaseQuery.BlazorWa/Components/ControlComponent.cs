using Microsoft.AspNetCore.Components;

namespace FSL.DatabaseQuery.BlazorWa.Components
{
    public class ControlComponent : ComponentBase
    {
        public ControlComponent()
        {
            IsValid = true;
        }

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool IsValid { get; set; }

        public string ValidCssClass => IsValid ? "" : "is-invalid";

        public string AllCssClass => $"form-control {CssClass ?? ""} {ValidCssClass}";

        public void ToggleDisabled()
        {
            Disabled = !Disabled;
        }
    }
}
