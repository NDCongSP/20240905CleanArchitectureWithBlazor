﻿using Domain.Entity.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using System.Reflection;

namespace WebUI.Pages
{

    //[Authorize(Policy = "Admin")]
    //[Authorize(Roles = "Admin,Users")]
    public partial class Products
    {
        List<Product> _products= new List<Product>();

        RadzenDataGrid<Product> _profileGrid;
        IEnumerable<int> _pageSizeOptions = new int[] { 5, 10, 20, 30 };
        bool _showPagerSummary = true;
        string _pagingSummaryFormat = "Displaying page {0} of {1} <b>(total {2} records)</b>";

        protected async override Task OnInitializedAsync()
        {
            var result = await _product.GetAllAsync();

            if (!result.Succeeded)
            {
                _notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "Did not connect to API.",
                    Duration = 2000
                });
            }

            _products = result.Data.ToList();
        }
    }
}
