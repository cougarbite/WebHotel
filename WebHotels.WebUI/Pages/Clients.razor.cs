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
    public partial class Clients
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

        protected IEnumerable<WebHotels.WebUI.Models.WebHotelsDB.Client> clients;

        protected RadzenDataGrid<WebHotels.WebUI.Models.WebHotelsDB.Client> grid0;
        protected override async Task OnInitializedAsync()
        {
            clients = await WebHotelsDBService.GetClients();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddClient>("Add Client", null);
            await grid0.Reload();
        }

        protected async Task EditRow(WebHotels.WebUI.Models.WebHotelsDB.Client args)
        {
            await DialogService.OpenAsync<EditClient>("Edit Client", new Dictionary<string, object> { {"Id", args.Id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, WebHotels.WebUI.Models.WebHotelsDB.Client client)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await WebHotelsDBService.DeleteClient(client.Id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete Client" 
                });
            }
        }
    }
}