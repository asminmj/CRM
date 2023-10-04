using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using B.Models.B1;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using B.Models;

namespace B.Pages
{
    public partial class CitizenshipComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

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
        protected SecurityService Security { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected B1Service B1 { get; set; }
        protected RadzenDataGrid<B.Models.B1.Citizenship> grid0;

        bool _hasChanges;
        protected bool hasChanges
        {
            get
            {
                return _hasChanges;
            }
            set
            {
                if (!object.Equals(_hasChanges, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "hasChanges", NewValue = value, OldValue = _hasChanges };
                    _hasChanges = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        bool _canEdit;
        protected bool canEdit
        {
            get
            {
                return _canEdit;
            }
            set
            {
                if (!object.Equals(_canEdit, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "canEdit", NewValue = value, OldValue = _canEdit };
                    _canEdit = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<B.Models.B1.Citizenship> _getCitizenshipsResult;
        protected IEnumerable<B.Models.B1.Citizenship> getCitizenshipsResult
        {
            get
            {
                return _getCitizenshipsResult;
            }
            set
            {
                if (!object.Equals(_getCitizenshipsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getCitizenshipsResult", NewValue = value, OldValue = _getCitizenshipsResult };
                    _getCitizenshipsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        B.Models.B1.Citizenship _citizenship;
        protected B.Models.B1.Citizenship citizenship
        {
            get
            {
                return _citizenship;
            }
            set
            {
                if (!object.Equals(_citizenship, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "citizenship", NewValue = value, OldValue = _citizenship };
                    _citizenship = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        bool _isEdit;
        protected bool isEdit
        {
            get
            {
                return _isEdit;
            }
            set
            {
                if (!object.Equals(_isEdit, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "isEdit", NewValue = value, OldValue = _isEdit };
                    _isEdit = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Security.InitializeAsync(AuthenticationStateProvider);
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }
        }
        protected async System.Threading.Tasks.Task Load()
        {
            hasChanges = false;

            canEdit = true;

            var b1GetCitizenshipsResult = await B1.GetCitizenships();
            getCitizenshipsResult = b1GetCitizenshipsResult;

            citizenship = b1GetCitizenshipsResult.FirstOrDefault();

            isEdit = true;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            citizenship = new B.Models.B1.Citizenship();

            isEdit = false;
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(B.Models.B1.Citizenship args)
        {
            isEdit = true;

            citizenship = args;
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var b1DeleteCitizenshipResult = await B1.DeleteCitizenship(data.ID);
                    if (b1DeleteCitizenshipResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception b1DeleteCitizenshipException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Citizenship" });
            }
        }

        protected async System.Threading.Tasks.Task Form0Submit(B.Models.B1.Citizenship args)
        {
            try
            {
                if (isEdit)
                {
                    var b1UpdateCitizenshipResult = await B1.UpdateCitizenship(citizenship.ID, citizenship);
                        NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Success,Summary = $"Success",Detail = $"Citizenship updated!" });


                }
            }
            catch (System.Exception b1UpdateCitizenshipException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Citizenship" });

            hasChanges = b1UpdateCitizenshipException is DbUpdateConcurrencyException;
            }

            try
            {
                if (!this.isEdit)
                {
                    var b1CreateCitizenshipResult = await B1.CreateCitizenship(args);
                    citizenship = new B.Models.B1.Citizenship();

                        NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Success,Summary = $"Success",Detail = $"Citizenship created!" });
                }
            }
            catch (System.Exception b1CreateCitizenshipException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to create new Citizenship!" });
            }
        }
    }
}
