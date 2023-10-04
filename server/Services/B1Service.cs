using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using B.Data;

namespace B
{
    public partial class B1Service
    {
        B1Context Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly B1Context context;
        private readonly NavigationManager navigationManager;

        public B1Service(B1Context context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public async Task ExportCitizenshipsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/b1/citizenships/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/b1/citizenships/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCitizenshipsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/b1/citizenships/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/b1/citizenships/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCitizenshipsRead(ref IQueryable<Models.B1.Citizenship> items);

        public async Task<IQueryable<Models.B1.Citizenship>> GetCitizenships(Query query = null)
        {
            var items = Context.Citizenships.AsQueryable();

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

            OnCitizenshipsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCitizenshipCreated(Models.B1.Citizenship item);
        partial void OnAfterCitizenshipCreated(Models.B1.Citizenship item);

        public async Task<Models.B1.Citizenship> CreateCitizenship(Models.B1.Citizenship citizenship)
        {
            OnCitizenshipCreated(citizenship);

            var existingItem = Context.Citizenships
                              .Where(i => i.ID == citizenship.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Citizenships.Add(citizenship);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(citizenship).State = EntityState.Detached;
                throw;
            }

            OnAfterCitizenshipCreated(citizenship);

            return citizenship;
        }

        partial void OnCitizenshipDeleted(Models.B1.Citizenship item);
        partial void OnAfterCitizenshipDeleted(Models.B1.Citizenship item);

        public async Task<Models.B1.Citizenship> DeleteCitizenship(int? id)
        {
            var itemToDelete = Context.Citizenships
                              .Where(i => i.ID == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCitizenshipDeleted(itemToDelete);

            Context.Citizenships.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCitizenshipDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnCitizenshipGet(Models.B1.Citizenship item);

        public async Task<Models.B1.Citizenship> GetCitizenshipById(int? id)
        {
            var items = Context.Citizenships
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            var itemToReturn = items.FirstOrDefault();

            OnCitizenshipGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.B1.Citizenship> CancelCitizenshipChanges(Models.B1.Citizenship item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCitizenshipUpdated(Models.B1.Citizenship item);
        partial void OnAfterCitizenshipUpdated(Models.B1.Citizenship item);

        public async Task<Models.B1.Citizenship> UpdateCitizenship(int? id, Models.B1.Citizenship citizenship)
        {
            OnCitizenshipUpdated(citizenship);

            var itemToUpdate = Context.Citizenships
                              .Where(i => i.ID == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(citizenship);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterCitizenshipUpdated(citizenship);

            return citizenship;
        }
    }
}
