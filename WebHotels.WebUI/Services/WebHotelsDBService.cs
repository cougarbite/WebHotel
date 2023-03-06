using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using WebHotels.WebUI.Data;

namespace WebHotels.WebUI
{
    public partial class WebHotelsDBService
    {
        WebHotelsDBContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly WebHotelsDBContext context;
        private readonly NavigationManager navigationManager;

        public WebHotelsDBService(WebHotelsDBContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportClientsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/webhotelsdb/clients/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/webhotelsdb/clients/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportClientsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/webhotelsdb/clients/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/webhotelsdb/clients/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnClientsRead(ref IQueryable<WebHotels.WebUI.Models.WebHotelsDB.Client> items);

        public async Task<IQueryable<WebHotels.WebUI.Models.WebHotelsDB.Client>> GetClients(Query query = null)
        {
            var items = Context.Clients.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnClientsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnClientGet(WebHotels.WebUI.Models.WebHotelsDB.Client item);

        public async Task<WebHotels.WebUI.Models.WebHotelsDB.Client> GetClientById(int id)
        {
            var items = Context.Clients
                              .AsNoTracking()
                              .Where(i => i.Id == id);

  
            var itemToReturn = items.FirstOrDefault();

            OnClientGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnClientCreated(WebHotels.WebUI.Models.WebHotelsDB.Client item);
        partial void OnAfterClientCreated(WebHotels.WebUI.Models.WebHotelsDB.Client item);

        public async Task<WebHotels.WebUI.Models.WebHotelsDB.Client> CreateClient(WebHotels.WebUI.Models.WebHotelsDB.Client client)
        {
            OnClientCreated(client);

            var existingItem = Context.Clients
                              .Where(i => i.Id == client.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Clients.Add(client);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(client).State = EntityState.Detached;
                throw;
            }

            OnAfterClientCreated(client);

            return client;
        }

        public async Task<WebHotels.WebUI.Models.WebHotelsDB.Client> CancelClientChanges(WebHotels.WebUI.Models.WebHotelsDB.Client item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnClientUpdated(WebHotels.WebUI.Models.WebHotelsDB.Client item);
        partial void OnAfterClientUpdated(WebHotels.WebUI.Models.WebHotelsDB.Client item);

        public async Task<WebHotels.WebUI.Models.WebHotelsDB.Client> UpdateClient(int id, WebHotels.WebUI.Models.WebHotelsDB.Client client)
        {
            OnClientUpdated(client);

            var itemToUpdate = Context.Clients
                              .Where(i => i.Id == client.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(client);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterClientUpdated(client);

            return client;
        }

        partial void OnClientDeleted(WebHotels.WebUI.Models.WebHotelsDB.Client item);
        partial void OnAfterClientDeleted(WebHotels.WebUI.Models.WebHotelsDB.Client item);

        public async Task<WebHotels.WebUI.Models.WebHotelsDB.Client> DeleteClient(int id)
        {
            var itemToDelete = Context.Clients
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnClientDeleted(itemToDelete);


            Context.Clients.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterClientDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportEmployeesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/webhotelsdb/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/webhotelsdb/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEmployeesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/webhotelsdb/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/webhotelsdb/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEmployeesRead(ref IQueryable<WebHotels.WebUI.Models.WebHotelsDB.Employee> items);

        public async Task<IQueryable<WebHotels.WebUI.Models.WebHotelsDB.Employee>> GetEmployees(Query query = null)
        {
            var items = Context.Employees.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnEmployeesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEmployeeGet(WebHotels.WebUI.Models.WebHotelsDB.Employee item);

        public async Task<WebHotels.WebUI.Models.WebHotelsDB.Employee> GetEmployeeById(int id)
        {
            var items = Context.Employees
                              .AsNoTracking()
                              .Where(i => i.Id == id);

  
            var itemToReturn = items.FirstOrDefault();

            OnEmployeeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEmployeeCreated(WebHotels.WebUI.Models.WebHotelsDB.Employee item);
        partial void OnAfterEmployeeCreated(WebHotels.WebUI.Models.WebHotelsDB.Employee item);

        public async Task<WebHotels.WebUI.Models.WebHotelsDB.Employee> CreateEmployee(WebHotels.WebUI.Models.WebHotelsDB.Employee employee)
        {
            OnEmployeeCreated(employee);

            var existingItem = Context.Employees
                              .Where(i => i.Id == employee.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Employees.Add(employee);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(employee).State = EntityState.Detached;
                throw;
            }

            OnAfterEmployeeCreated(employee);

            return employee;
        }

        public async Task<WebHotels.WebUI.Models.WebHotelsDB.Employee> CancelEmployeeChanges(WebHotels.WebUI.Models.WebHotelsDB.Employee item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEmployeeUpdated(WebHotels.WebUI.Models.WebHotelsDB.Employee item);
        partial void OnAfterEmployeeUpdated(WebHotels.WebUI.Models.WebHotelsDB.Employee item);

        public async Task<WebHotels.WebUI.Models.WebHotelsDB.Employee> UpdateEmployee(int id, WebHotels.WebUI.Models.WebHotelsDB.Employee employee)
        {
            OnEmployeeUpdated(employee);

            var itemToUpdate = Context.Employees
                              .Where(i => i.Id == employee.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(employee);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEmployeeUpdated(employee);

            return employee;
        }

        partial void OnEmployeeDeleted(WebHotels.WebUI.Models.WebHotelsDB.Employee item);
        partial void OnAfterEmployeeDeleted(WebHotels.WebUI.Models.WebHotelsDB.Employee item);

        public async Task<WebHotels.WebUI.Models.WebHotelsDB.Employee> DeleteEmployee(int id)
        {
            var itemToDelete = Context.Employees
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEmployeeDeleted(itemToDelete);


            Context.Employees.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEmployeeDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}