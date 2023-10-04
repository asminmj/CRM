using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using B.Models.B1;
using Microsoft.AspNetCore.Identity;
using B.Models;
using B.Pages;

namespace B.Layouts
{
    public partial class MainLayoutComponent : LayoutComponentBase
    {
        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected B1Service B1 { get; set; }

        protected RadzenBody body0;
        protected RadzenSidebar sidebar0;

        private void Authenticated()
        {
             StateHasChanged();
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
             if (Security != null)
             {
                  Security.Authenticated += Authenticated;

                  await Security.InitializeAsync(AuthenticationStateProvider);
             }
        }

        protected async System.Threading.Tasks.Task SidebarToggle0Click(dynamic args)
        {
            await InvokeAsync(() => { sidebar0.Toggle(); });

            await InvokeAsync(() => { body0.Toggle(); });
        }
    }
}
