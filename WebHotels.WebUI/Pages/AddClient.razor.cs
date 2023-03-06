using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace WebHotels.WebUI.Pages
{
    public partial class AddClient
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public WebHotelsDBService WebHotelsDBService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            client = new WebHotels.WebUI.Models.WebHotelsDB.Client();
        }
        protected bool errorVisible;
        protected WebHotels.WebUI.Models.WebHotelsDB.Client client;

        protected async Task FormSubmit()
        {
            try
            {
                await WebHotelsDBService.CreateClient(client);
                DialogService.Close(client);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}