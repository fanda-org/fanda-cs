#pragma checksum "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\PartyCategories\IndexEdit.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e1485fc6cd4d92d3a243271d5d5f13ce7f441430"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_PartyCategories_IndexEdit), @"mvc.1.0.view", @"/Views/PartyCategories/IndexEdit.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e1485fc6cd4d92d3a243271d5d5f13ce7f441430", @"/Views/PartyCategories/IndexEdit.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"257665fb021dc14e5ac258eb7cb6e8ad8ec20aff", @"/Views/_ViewImports.cshtml")]
    public class Views_PartyCategories_IndexEdit : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
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
#line 1 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\PartyCategories\IndexEdit.cshtml"
  
    ViewData["Title"] = "Contact Categories";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            DefineSection("styles", async() => {
                WriteLiteral("\r\n    <style>\r\n        .hide {\r\n            display: none;\r\n        }\r\n    </style>\r\n");
            }
            );
            WriteLiteral("\r\n");
            WriteLiteral("<div class=\"card\">\r\n    <div class=\"card-header\">\r\n        <h3 class=\"card-title\">Contact Categories</h3>\r\n    </div>\r\n    <div class=\"card-body\">\r\n");
            WriteLiteral("        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e1485fc6cd4d92d3a243271d5d5f13ce7f4414304878", async() => {
                WriteLiteral("\r\n            <div id=\"jsGrid\"></div>\r\n        ");
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
            WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n\r\n");
            DefineSection("scripts", async() => {
                WriteLiteral(@"
    <script>
        require(['jquery', 'jsgrid', 'sweetalert2'], function ($, jsGrid, Swal) {
            //console.log($, grid, polyfill, Swal);
            $(document).ajaxSend(function (e, xhr, options) {
                if (options.type.toUpperCase() == ""POST"" || options.type.toUpperCase() == ""PATCH"") {
                    var token = $(""input[name='__RequestVerificationToken']"").val();
                    xhr.setRequestHeader(""RequestVerificationToken"", token);
                }
            });

            var $grid = $(""#jsGrid"").jsGrid({
                height: ""auto"",
                width: ""100%"",

                heading: true,
                filtering: true,

                inserting: true,
                editing: true,
                selecting: true,
                sorting: true,

                paging: true,
                pageLoading: false,
                autoload: true,
                pageSize: 10,
                pageButtonCount: 3,

                no");
                WriteLiteral(@"DataContent: ""No records"",

                confirmDeleting: false,
                deleteConfirm: function () {
                    return ""Do you really want to delete the category?"";
                },
                //data: clients,
                controller: {
                    loadData: function (filter) {
                        return $.ajax({
                            type: ""GET"",
                            url: """);
#nullable restore
#line 94 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\PartyCategories\IndexEdit.cshtml"
                             Write(Url.Action("GetAll"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@""",
                            data: filter                            
                        });
                    },

                    insertItem: function (item) {
                        var d = $.Deferred();

                        $.ajax({
                            type: 'POST',
                            url: '");
#nullable restore
#line 104 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\PartyCategories\IndexEdit.cshtml"
                             Write(Url.Action("Save"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"',
                            data: item                            
                        }).done(function (response) {
                            console.log(""done:"", response);
                            d.resolve(response);
                        }).fail(function (message) {
                            console.log(""fail"", message);
                            d.reject();
                        });

                        return d.promise();
                    },

                    updateItem: function (item) {
                        return $.ajax({
                            type: ""POST"",
                            url: """);
#nullable restore
#line 120 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\PartyCategories\IndexEdit.cshtml"
                             Write(Url.Action("Save"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@""",
                            data: item
                        });
                    },

                    deleteItem: function (item) {
                        return $.ajax({
                            type: ""POST"",
                            url: """);
#nullable restore
#line 128 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\PartyCategories\IndexEdit.cshtml"
                             Write(Url.Action("Delete"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@""",
                            data: item
                        });
                    }
                },
                invalidNotify: function (args) {
                    //console.log(""invalidNotify"", args);
                    var messages = $.map(args.errors, function (error) {
                        return error.field.name + "": "" + error.message;
                    });
                    console.log(""invalidNotify"", messages);
                    //var err = $(""#errors"").html(messages);
                    //err.removeClass(""hide"");
                    //swal(messages, { icon: 'error' });
                },
                onItemInvalid: function (args) {
                    var messages = $.map(args.errors, function (error) {
                        return error.field.name + "": "" + error.message;
                    });
                    console.log(""onItemInvalid"", messages);

                    const wrapper = document.createElement('div');
                    wrapp");
                WriteLiteral(@"er.innerHTML = messages.join(""<br>"");
                    Swal.fire({ title: 'Error', html: wrapper, type: 'error' });
                },
                onItemDeleting: function (args) {
                    if (!args.item.deleteConfirmed) { // custom property for confirmation
                        args.cancel = true; // cancel deleting
                        Swal.fire({
                            title: 'Are you sure?',
                            text: ""Do you want to delete this Category? Once deleted, you will not be able to recover!"",
                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#d33',
                            confirmButtonText: 'Yes, delete it!',
                            reverseButtons: true
                        }).then((result) => {
                            if (result.value) {
                                args.");
                WriteLiteral(@"item.deleteConfirmed = true;
                                $grid.jsGrid('deleteItem', args.item); //call deleting once more in callback
                                Swal.fire('Deleted!', 'Category has been deleted.',  'success');
                            }
                        });
                        //confirm.showConfirm('Are you sure?', function() {
                        //    args.item.deleteConfirmed = true;
                        //    $grid.jsGrid('deleteItem', args.item); //call deleting once more in callback
                        //});
                    }
                },
                onError: function (args) {
                    //debugger;
                    //swal(args.errors[0].message, { icon: 'error' });
                    //console.log(""onError"", args);
                    //var err = $(""#errors"").html(args.args[1] + "": Error occured!"");
                    //err.removeClass(""hide"");
                    Swal.fire('Error', 'Error occured!', 'error')");
                WriteLiteral(@";
                },
                fields: [
                    {
                        name: ""code"", type: ""text"", title: ""Code"", width: 75,
                        validate: {
                            validator: function (value, item) {
                                if (value == undefined || value == null || value == """")
                                    return false;

                                var gridData = $grid.jsGrid(""option"", ""data"");
                                var editRow = $grid.jsGrid(""option"", ""_editingRow"");
                                var editItem = undefined;
                                if (editRow != null)
                                    editItem = editRow.data(""JSGridItem"");
                                //console.log(""editingItem"", editItem, item);
                                //debugger;
                                for (i = 0; i < gridData.length; i++) {
                                    if (editItem == undefined) {
          ");
                WriteLiteral(@"                              if (value.toLowerCase() == gridData[i].code.toLowerCase())
                                            return false;
                                    }
                                    else {
                                        if (editItem.categoryId == gridData[i].categoryId)
                                            continue;
                                        if (value.toLowerCase() == gridData[i].code.toLowerCase())
                                            return false;
                                    }
                                }
                                //clearError();
                                return true;
                            },
                            message: function (value, item) {
                                if (value == undefined || value == null || value == """")
                                    return ""should not be empty"";
                                else
                            ");
                WriteLiteral(@"        return ""'"" + value + ""' already exists"";
                            },
                            param: undefined
                        }
                    },
                    {
                        name: ""name"", type: ""text"", title: ""Name"", width: 150,
                        validate: {
                            validator: function (value, item) {
                                if (value == undefined || value == null || value == """")
                                    return false;
                                var gridData = $(""#jsGrid"").jsGrid(""option"", ""data"");
                                var editRow = $(""#jsGrid"").jsGrid(""option"", ""_editingRow"");
                                var editItem = undefined;
                                if (editRow != null)
                                    editItem = editRow.data(""JSGridItem"");
                                //console.log(""editingItem"", editItem, item);
                                //debugger;
       ");
                WriteLiteral(@"                         for (i = 0; i < gridData.length; i++) {
                                    if (editItem == undefined) {
                                        if (value.toLowerCase() == gridData[i].name.toLowerCase())
                                            return false;
                                    }
                                    else {
                                        if (editItem.categoryId == gridData[i].categoryId)
                                            continue;
                                        if (value.toLowerCase() == gridData[i].name.toLowerCase())
                                            return false;
                                    }
                                }
                                //clearError();
                                return true;
                            },
                            message: function (value, item) {
                                if (value == undefined || value == null || value");
                WriteLiteral(@" == """")
                                    return ""should not be empty"";
                                else
                                    return ""'"" + value + ""' already exists"";
                            },
                            param: undefined
                        }
                    },
                    { name: ""description"", type: ""text"", title: ""Description"", width: 200 },
                    {
                        name: ""active"", type: ""checkbox"", title: ""Active"", sorting: false,
                        insertTemplate: function () {
                            var input = this.__proto__.insertTemplate.call(this); //original input
                            input.prop('checked', true);
                            return input;
                        }
                    },
                    { name: ""categoryId"", visible: false, width: 0 },
                    //{ name: ""Age"", type: ""number"", width: 50 },
                    //{ name: ""Address"", type: ""t");
                WriteLiteral(@"ext"", width: 200 },
                    //{ name: ""Country"", type: ""select"", items: countries, valueField: ""Id"", textField: ""Name"" },
                    { type: ""control"" }
                ]
            });
            $(""#jsGrid"").find("".jsgrid-mode-button"").click();
            //function clearError() {
            //    var err = $(""#errors"").html("""");
            //    err.addClass(""hide"");
            //}
        });
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
