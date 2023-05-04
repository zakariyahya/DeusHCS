// using System.Globalization;
// using Microsoft.AspNetCore.Components;
// using Radzen;

// namespace WebAdmin.Pages.Visualizations
// {
//     public partial class Visualizations
//     {
//     bool showDataLabels = false;

//     void OnSeriesClick(SeriesClickEventArgs args)
//     {
//         // console.Log(args);
//     }

//     class DataItem
//     {
//         public string Quarter { get; set; }
//         public double Revenue { get; set; }
//     }

//     string FormatAsUSD(object value)
//     {
//         return ((double)value).ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
//     }

//     DataItem[] revenue2019 = new DataItem[]
//     {
//     new DataItem
//     {
//         Quarter = "Q1",
//         Revenue = 234000
//     },
//     new DataItem
//     {
//         Quarter = "Q2",
//         Revenue = 284000
//     },
//     new DataItem
//     {
//         Quarter = "Q3",
//         Revenue = 274000
//     },
//     new DataItem
//     {
//         Quarter = "Q4",
//         Revenue = 294000
//     },
//     };

//     DataItem[] revenue2020 = new DataItem[] {
//     new DataItem
//     {
//     Quarter = "Q1",
//     Revenue = 254000
//     },
//     new DataItem
//     {
//     Quarter = "Q2",
//     Revenue = 324000
//     },
//     new DataItem
//     {
//     Quarter = "Q3",
//     Revenue = 354000
//     },
//     new DataItem
//     {
//     Quarter = "Q4",
//     Revenue = 394000
//     },

//     };
//     }
// }