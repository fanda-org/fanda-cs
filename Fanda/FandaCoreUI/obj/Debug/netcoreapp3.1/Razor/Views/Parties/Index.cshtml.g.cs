#pragma checksum "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2407384cf8ca8c365c27bf218004f4bbd19bb85e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Parties_Index), @"mvc.1.0.view", @"/Views/Parties/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\_ViewImports.cshtml"
using Fanda.Mvc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\_ViewImports.cshtml"
using Fanda.Shared.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\_ViewImports.cshtml"
using Fanda.Dto;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\_ViewImports.cshtml"
using Fanda.Shared.Enums;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\_ViewImports.cshtml"
using Fanda.Shared.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\_ViewImports.cshtml"
using FandaCoreUI.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
using System.Text;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2407384cf8ca8c365c27bf218004f4bbd19bb85e", @"/Views/Parties/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"19f07351583c639af577b61754b3ed16299ad254", @"/Views/_ViewImports.cshtml")]
    public class Views_Parties_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/vendor/bootstrap4c-custom-switch/css/component-custom-switch.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_breadcrumb-items", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_breadcrumb-menu", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            DefineSection("Styles", async() => {
                WriteLiteral("\r\n");
                WriteLiteral("    <link href=\"https://cdn.datatables.net/1.10.20/css/dataTables.bootstrap4.min.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "2407384cf8ca8c365c27bf218004f4bbd19bb85e6614", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
            WriteLiteral("\r\n");
            DefineSection("Breadcrumbs", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 14 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
      
        var breadcrumbs = new Dictionary<string, string>()
{
{"Home","/Home" },
{"Contacts","/Parties" },
};
        ViewBag.Breadcrumbs = breadcrumbs;
    

#line default
#line hidden
#nullable disable
                WriteLiteral("    <ol class=\"breadcrumb\">\r\n        <!-- BREADCRUMB-ITEMS -->\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "2407384cf8ca8c365c27bf218004f4bbd19bb85e8400", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_2.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n        <!-- /BREADCRUMB-ITEMS -->\r\n        <!-- BREADCRUMB-MENU -->\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "2407384cf8ca8c365c27bf218004f4bbd19bb85e9658", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_3.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n        <!-- /BREADCRUMB-MENU -->\r\n    </ol>\r\n");
            }
            );
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2407384cf8ca8c365c27bf218004f4bbd19bb85e10946", async() => {
                WriteLiteral(@"
    <div class=""card rounded"">
        <div class=""card-header"">
            <div class=""row"">
                <div class=""col-6 mt-sm-auto mb-sm-auto"">
                    <span class=""font-sm""><i class=""icon-briefcase""></i> Contacts</span>
                </div>
                <div class=""col-6"">
                    <div class=""float-right"">
                        <a class=""btn btn-outline-primary font-weight-bold btn-sm"" role=""button"" data-toggle=""tooltip"" title=""Refresh"" href=""#"" onclick=""$('#table').DataTable().ajax.reload();""><i class=""icon-refresh""></i></a>
                        <a class=""btn btn-primary dropdown-toggle my-dropdown my-tooltip btn-sm"" href=""#"" role=""button"" id=""dropdownMenuLink"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"" title=""Create New""><i class=""icon-note""></i> Create New</a>
                        <div class=""dropdown-menu"" aria-labelledby=""dropdownMenuLink"">
");
#nullable restore
#line 44 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
                             foreach (var item in FandaEnums.GetPartyTypes())
                            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                <a class=\"dropdown-item\"");
                BeginWriteAttribute("href", " href=\"", 2115, "\"", 2171, 1);
#nullable restore
#line 46 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
WriteAttributeValue("", 2122, Url.Action("Create", new { contactType = item }), 2122, 49, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">");
#nullable restore
#line 46 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
                                                                                                             Write(item);

#line default
#line hidden
#nullable disable
                WriteLiteral("</a>\r\n");
#nullable restore
#line 47 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
                            }

#line default
#line hidden
#nullable disable
                WriteLiteral(@"                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class=""card-body"">
            <table id=""table"" class=""table table-sm table-align-middle table-hover"" style=""width: 100%"">
                <thead>
                    <tr>
                        <th>Code</th>
                        <th>Name</th>
                        <th>Type</th>
                        <th>Category</th>
                        <th>Active</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <th>Code</th>
                        <th>Name</th>
                        <th>Type</th>
                        <th>Category</th>
                        <th>Active</th>
                        <th></th>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            DefineSection("scripts", async() => {
                WriteLiteral("\r\n");
                WriteLiteral(@"    <script src=""https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js""></script>
    <script src=""https://cdn.datatables.net/1.10.20/js/dataTables.bootstrap4.min.js""></script>

    <script type=""text/javascript"">
        $(function () {
            $('.my-dropdown').dropdown();
            $('.my-dropdown').tooltip();

            // Add Searchable footer
            $('#table tfoot th').each(function () {
                var title = $(this).text();

                if (title !== '') {
                    if (title == 'Active') {
                        $(this).html('<select class=""form-control form-control-sm"" style=""width: 100%"" placeholder=""Search ' + title + '"">' +
                            '<option value=""1"">Active</option> <option value=""0"">Inactive</option> <option value="""" selected>Both</option>' +
                            '</select>');
                    }
                    else if (title == 'Type') {
                        //StringBuilder opt = new StringBuilder");
                WriteLiteral(@"();
                        //foreach (var item in FandaEnums.GetPartyTypes())
                        //{
                        //    opt.Append($""<option value=\""{item.ToString()}\"">{item.ToString()}</option>"");
                        //}
                        $(this).html('<select class=""form-control form-control-sm"" style=""width: 100%"" placeholder=""Search ' + title + '"">' +
                            '<option value="""" selected>All</option>' +
                            '<option value=""Customer"">Customer</option> <option value=""Supplier"">Supplier</option>' +
                            '<option value=""Buyer"">Buyer</option><option value=""Other"">Other</option>' +
                            '</select>');
                    }
                    else
                        $(this).html('<input type=""text"" class=""form-control form-control-sm"" style=""width: 100%"" placeholder=""Search ' + title + '"" />');
                }
            });

            $(document).ajaxSend(function (e, xhr");
                WriteLiteral(@", options) {
                if (options.type.toUpperCase() == ""POST"" || options.type.toUpperCase() == ""PATCH"") {
                    var token = $(""input[name='__RequestVerificationToken']"").val();
                    xhr.setRequestHeader(""RequestVerificationToken"", token);
                }
            });

            $('#table').on('change', 'input[type=""checkbox""]', function (e) {
                // stop post
                e.preventDefault();

                var data = {};
                data.id = $(this).data(""id"");
                data.active = $(this).prop('checked');
                console.log(""Active:Checkbox"", data);

                //if (id !== '') {
                $.ajax({
                    type: 'PATCH',
                    url: """);
#nullable restore
#line 138 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
                     Write(Url.Action("ChangeStatus"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@""",
                    data: JSON.stringify(data),
                    contentType: ""application/json; charset=utf-8"",
                    //dataType: 'json',
                    success: function (res, textStatus, jqXhr) {
                        console.log('Status successfully patched');
                    },
                    error: function (res, textStatus, error) {
                        console.log(""Error:"", res, textStatus, error);
                    }
                }).done(function (res) {
                    console.log(""Done"");
                        console.log(res);
                    }).fail(function (res) {
                        console.log('Fail');
                        console.log(res);
                    });
            });

            var table = $('#table').DataTable({
                autoWidth: true,
                processing: true,
                serverSide: true,
                order: [[0, ""asc""]],
                pageLength: 10,
             ");
                WriteLiteral(@"   stateSave: true,
                stateSaveParams: function (settings, data) {
                    var temp = {};
                    $('#table tfoot input').each(function (n) {
                        temp[$(this).attr('placeholder')] = this.value;
                    });
                    $('#table tfoot select').each(function (n) {
                        temp[$(this).attr('placeholder')] = this.value;
                    });
                    data.colsFilter = temp;
                },
                stateLoadParams: function (settings, data) {
                    $.each(data.colsFilter, function (key, val) {
                        if (key == 'Search Active' || key == 'Search Type') {
                            $('#table tfoot select[placeholder=""' + key + '""]').val(val);
                        }
                        else {
                            $('#table tfoot input[placeholder=""' + key + '""]').val(val);
                        }
                    });
             ");
                WriteLiteral("   },\r\n                ajax: {\r\n                    url: \"");
#nullable restore
#line 185 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
                     Write(Url.Action("GetAll"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"""
                },
                deferRender: true,
                //scrollY: ""400px"",
                //scrollX: true,
                search: { caseInsensitive: true },
                columnDefs: [
                    {
                        targets: 0, data: 'Code',
                        render: function (code, type, row, meta) {
                            var catId = row.PartyId;
                            var link = '");
#nullable restore
#line 196 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
                                   Write(Url.Action("Details", new { id = "-1" }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"';
                            link = link.replace(""-1"", catId);
                            return '<a href=' + link + '>' + code + '</a>';
                        }
                    },
                    {
                        targets: 1, data: 'Name',
");
                WriteLiteral(@"                    },
                    {
                        targets: 2, data: 'PartyType'
                    },
                    {
                        targets: 3, data: 'CategoryName'
                    },
                    {
                        targets: 4, data: 'Active',
                        render: function (active, type, party, meta) {
                                //return '<span class=""' + activeClass(active) + '"">' + (active == 0 ? ""Inactive"" : ""Active"") + '</span>';
                                //return data == 0 ? 'Inactive' : 'Active';
                                //return '<div class=""custom-control custom-checkbox"">' +
                                //    '<input type=""checkbox"" ' + (active ? ""checked"" : """") + ' class=""custom-control-input"" id=""chk-' + cat.CategoryId + '"">' +
                                //    '<label class=""custom-control-label"" style=""cursor:pointer"" for=""chk-' + cat.CategoryId + '"">&nbsp;</label>'
                           ");
                WriteLiteral(@"     //'</div>';
                                //return '<input type=""checkbox"" id=""chk-' + cat.CategoryId + '"" ' + (active ? ""checked"" : """") + ' data-toggle=""toggle"" data-size=""small"" data-on=""Active"" data-off=""Inactive"" data-onstyle=""primary"" data-offstyle=""secondary"">';
                            return '<div class=""custom-switch custom-switch-xs"">' +
                                '<input class=""custom-switch-input"" id=""chk-' + party.PartyId + '"" ' + (active ? ""checked"" : """") + ' data-id=""' + party.PartyId +'"" type=""checkbox"">' +
                                '<label class=""custom-switch-btn"" for=""chk-' + party.PartyId + '""></label>' +
                                '</div>';
                            }
                    },
                    {
                        targets: 5, data: 'PartyId',
                        render: function (id, type, row, meta) {

                            var editLink = '");
#nullable restore
#line 236 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
                                       Write(Url.Action("Edit", new { id = "-1" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\';\r\n                            var deleteLink = \'");
#nullable restore
#line 237 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\Parties\Index.cshtml"
                                         Write(Url.Action("Delete", new { id = "-1" }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"';
                            editLink = editLink.replace(""-1"", id);
                            deleteLink = deleteLink.replace(""-1"", id);

                            return '<div class=""btn-group"" role=""group"">' +
                                '<a class=""btn btn-sm btn-outline-primary font-weight-bold"" title=""Edit"" data-toggle=""tooltip"" href=' + editLink + '><i class=""icon-pencil""></i></a>' +
                                '<a class=""btn btn-sm btn-outline-danger font-weight-bold"" title=""Delete"" data-toggle=""tooltip"" href=' + deleteLink + '><i class=""icon-trash""></i></a>' +
                                '</div>';
                        },
                        sortable: false,
                        searchable: false
                    }
                ]
            });

            // Apply the search
            table.columns().every(function () {
                var that = this;

                $('input', this.footer()).on('keyup change', function () {
                  ");
                WriteLiteral(@"  if (that.search() !== this.value) {
                        that
                            .search(this.value, false, true, false)
                            .draw();
                    }
                });

                $('select', this.footer()).on('change', function () {
                    if (that.search() !== this.value) {
                        //var val = $.fn.dataTable.util.escapeRegex($(this).val());
                        that
                            .search(this.value, false, true, false)
                            .draw();
                    }
                });
            });

            $('body').tooltip({ selector: '[data-toggle=""tooltip""]' });
            //$('[data-toggle=""tooltip""]').tooltip({ container: 'body' });
        });

        function RefreshTable(tableId, urlData) {
            console.log(""Refresh table started..."");
            $.getJSON(urlData, null, function (json) {
                table = $(tableId).dataTable();
               ");
                WriteLiteral(@" oSettings = table.fnSettings();

                table.fnClearTable(this);

                for (var i = 0; i < json.aaData.length; i++) {
                    table.oApi._fnAddData(oSettings, json.aaData[i]);
                }

                oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
                table.fnDraw();
            });
        }

        function activeClass(value) {
            return ""badge "" + (value ? ""badge-success"" : ""badge-secondary"");
        }
    </script>
");
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
