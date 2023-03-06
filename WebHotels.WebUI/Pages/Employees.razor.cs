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
    public partial class Employees
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

        protected IEnumerable<WebHotels.WebUI.Models.WebHotelsDB.Employee> employees;

        protected RadzenDataGrid<WebHotels.WebUI.Models.WebHotelsDB.Employee> grid0;
        protected override async Task OnInitializedAsync()
        {
            employees = await WebHotelsDBService.GetEmployees();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddEmployee>("Add Employee", null);
            await grid0.Reload();
        }

        protected async Task EditRow(WebHotels.WebUI.Models.WebHotelsDB.Employee args)
        {
            await DialogService.OpenAsync<EditEmployee>("Edit Employee", new Dictionary<string, object> { {"Id", args.Id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, WebHotels.WebUI.Models.WebHotelsDB.Employee employee)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await WebHotelsDBService.DeleteEmployee(employee.Id);

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
                    Detail = $"Unable to delete Employee" 
                });
            }
        }
    }
}