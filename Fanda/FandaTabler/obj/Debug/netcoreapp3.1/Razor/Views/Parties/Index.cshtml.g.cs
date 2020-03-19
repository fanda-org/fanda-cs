#pragma checksum "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "db54858501cb7d1040f5adf2e5f836120352fd41"
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
#line 1 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\_ViewImports.cshtml"
using FandaTabler;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\_ViewImports.cshtml"
using FandaTabler.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\_ViewImports.cshtml"
using Fanda.Dto;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\_ViewImports.cshtml"
using Fanda.Dto.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\_ViewImports.cshtml"
using Fanda.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
using System.Text;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"db54858501cb7d1040f5adf2e5f836120352fd41", @"/Views/Parties/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"257665fb021dc14e5ac258eb7cb6e8ad8ec20aff", @"/Views/_ViewImports.cshtml")]
    public class Views_Parties_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral(@"<div class=""card"">
    <div class=""card-header"">
        <h3 class=""card-title""><i class=""fe fe-users""></i> Business Contacts</h3>
        <div class=""card-options"">
            <a class=""btn btn-sm btn-outline-primary mr-1"" role=""button"" data-toggle=""tooltip"" title=""Refresh"" href=""#"" onclick=""$('#table').DataTable().ajax.reload();""><i class=""fe fe-refresh-cw""></i></a>
            <a class=""btn btn-sm btn-primary dropdown-toggle dropdown-toggle-split my-dropdown my-tooltip"" href=""#"" role=""button"" id=""dropdownMenuLink"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"" title=""Create New""><i class=""fe fe-edit""></i> Create New</a>
            <div class=""dropdown-menu"" aria-labelledby=""dropdownMenuLink"">
");
#nullable restore
#line 18 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
                 foreach (var item in FandaEnums.GetPartyTypes())
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <a class=\"btn dropdown-item\"");
            BeginWriteAttribute("href", " href=\"", 1053, "\"", 1109, 1);
#nullable restore
#line 20 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
WriteAttributeValue("", 1060, Url.Action("Create", new { contactType = item }), 1060, 49, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 20 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
                                                                                                     Write(item);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n");
#nullable restore
#line 21 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\r\n        </div>\r\n    </div>\r\n");
            WriteLiteral("    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "db54858501cb7d1040f5adf2e5f836120352fd416674", async() => {
                WriteLiteral(@"
        <div class=""table-responsive"">
            <table id=""table"" class=""table table-sm table-striped table-align-middle table-hover card-table table-vcenter text-nowrap datatable"">
                <!---->
                <thead>
                    <tr>
                        <th class=""w-8"">Code</th>
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
    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            WriteLiteral("</div>\r\n\r\n\r\n");
            DefineSection("scripts", async() => {
                WriteLiteral(@"
    <script type=""text/javascript"">
        require(['jquery', 'datatables', 'sweetalert2'], function ($, dataTable, Swal) {
            $('.my-dropdown').dropdown();
            $('.my-dropdown').tooltip();

            // Add Searchable footer
            $('#table tfoot th').each(function () {
                var title = $(this).text();

                if (title !== '') {
                    if (title == 'Active') {
                        $(this).html('<select class=""form-control"" style=""width: 100%"" placeholder=""Search ' + title + '"">' +
                            '<option value=true>Active</option>' +
                            '<option value=false>Inactive</option>' +
                            '<option value="""" selected>Both</option>' +
                            '</select>');
                    }
                    else if (title == 'Type') {
                        //StringBuilder opt = new StringBuilder();
                        //foreach (var item in FandaEnums.GetPart");
                WriteLiteral(@"yTypes())
                        //{
                        //    opt.Append($""<option value=\""{item.ToString()}\"">{item.ToString()}</option>"");
                        //}
                        $(this).html('<select class=""form-control"" style=""width: 100%"" placeholder=""Search ' + title + '"">' +
                            '<option value="""" selected>All</option>' +
                            '<option value=""Customer"">Customer</option>' +
                            '<option value=""Supplier"">Supplier</option>' +
                            '<option value=""Buyer"">Buyer</option>' +
                            '<option value=""Other"">Other</option>' +
                            '</select>');
                    }
                    else
                        $(this).html('<input type=""text"" class=""form-control"" style=""width: 100%"" placeholder=""Search ' + title + '"" />');
                }
            });

            // Antiforgery Token
            $(document).ajaxSend(function (e, xhr,");
                WriteLiteral(@" options) {
                if (options.type.toUpperCase() == ""POST"" || options.type.toUpperCase() == ""PATCH"") {
                    var token = $(""input[name='__RequestVerificationToken']"").val();
                    xhr.setRequestHeader(""RequestVerificationToken"", token);
                }
            });

            // Active checkbox
            $('#table').on('change', 'input[type=""checkbox""]', function (e) {
                // stop post
                e.preventDefault();

                var data = {};
                data.id = $(this).data(""id"");
                data.active = $(this).prop('checked');
                console.log(""Active:Checkbox"", data);

                $.ajax({
                    type: 'PATCH',
                    url: """);
#nullable restore
#line 114 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
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

            // DataTable creation
            var table = $('#table').DataTable({
                autoWidth: true,
                processing: true,
                serverSide: true,
                order: [[0, ""asc""]],
                pageLength: 10,");
                WriteLiteral(@"
                stateSave: true,
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
                    });");
                WriteLiteral("\r\n                },\r\n                ajax: {\r\n                    url: \"");
#nullable restore
#line 162 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
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
#line 173 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
                                   Write(Url.Action("Details", new { id = "-1" }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"';
                            link = link.replace(""-1"", catId);
                            return '<a href=' + link + ' class=""text-inherit"">' + code + '</a>';
                        }
                    },
                    {
                        targets: 1, data: 'Name',
                        render: function (name, type, row, meta) {
                            var catId = row.PartyId;
                            var link = '");
#nullable restore
#line 182 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
                                   Write(Url.Action("Details", new { id = "-1" }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"';
                            link = link.replace(""-1"", catId);
                            return '<a href=' + link + ' class=""text-inherit"">' + name + '</a>';
                        }
                    },
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
                                //    '<input type=""checkbox"" ' + (active ? ""checked"" : """") + ' class=""custom-control-input"" id=""chk-' + cat.Cate");
                WriteLiteral(@"goryId + '"">' +
                                //    '<label class=""custom-control-label"" style=""cursor:pointer"" for=""chk-' + cat.CategoryId + '"">&nbsp;</label>'
                                //'</div>';
                                //return '<input type=""checkbox"" id=""chk-' + cat.CategoryId + '"" ' + (active ? ""checked"" : """") + ' data-toggle=""toggle"" data-size=""small"" data-on=""Active"" data-off=""Inactive"" data-onstyle=""primary"" data-offstyle=""secondary"">';
                            return '<label class=""custom-switch"">' +
                                '<input type=""checkbox"" class=""custom-switch-input"" name=""chk-' + party.PartyId + '"" ' + (active ? ""checked"" : """") + ' data-id=""' + party.PartyId + '"">' +
                                '<span class=""custom-switch-indicator""></span>' +
                                '<span class=""custom-switch-description"">&nbsp;</span>' +
                                '</label>';
                            }
                    },
                    {");
                WriteLiteral("\n                        targets: 5, data: \'PartyId\',\r\n                        render: function (id, type, row, meta) {\r\n                            var editLink = \'");
#nullable restore
#line 213 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
                                       Write(Url.Action("Edit", new { id = "-1" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\';\r\n                            var deleteLink = \'");
#nullable restore
#line 214 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Parties\Index.cshtml"
                                         Write(Url.Action("Delete", new { id = "-1" }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"';
                            editLink = editLink.replace(""-1"", id);
                            deleteLink = deleteLink.replace(""-1"", id);

                            return '<div class=""btn-group"" role=""group"">' +
                                '<a class=""btn btn-sm btn-outline-primary font-weight-bold"" title=""Edit"" data-toggle=""tooltip"" href=' + editLink + '><i class=""fe fe-edit-2""></i></a>' +
                                '<a class=""btn btn-sm btn-outline-danger font-weight-bold"" title=""Delete"" data-toggle=""tooltip"" href=' + deleteLink + '><i class=""fe fe-trash""></i></a>' +
                                '</div>';
                        },
                        sortable: false,
                        searchable: false
                    }
                ]
            });

            // Apply search
            table.columns().every(function () {
                var that = this;

                $('input', this.footer()).on('keyup change', function () {
                    ");
                WriteLiteral(@"if (that.search() !== this.value) {
                        that
                            .search(this.value, false, true, false)
                            .draw();
                    }
                });

                $('select', this.footer()).on('change', function () {
                    debugger;
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
                table = $(tableId).");
                WriteLiteral(@"dataTable();
                oSettings = table.fnSettings();

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
