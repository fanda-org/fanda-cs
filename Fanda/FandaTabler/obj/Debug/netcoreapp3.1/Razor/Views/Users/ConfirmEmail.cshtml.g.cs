#pragma checksum "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Users\ConfirmEmail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "104bb10fc6285ba86bdd6e45aa1d04bddca920a7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Users_ConfirmEmail), @"mvc.1.0.view", @"/Views/Users/ConfirmEmail.cshtml")]
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
using Fanda.Shared.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\_ViewImports.cshtml"
using Fanda.Shared.Enums;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\_ViewImports.cshtml"
using Fanda.Shared.Extensions;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"104bb10fc6285ba86bdd6e45aa1d04bddca920a7", @"/Views/Users/ConfirmEmail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2eb0a75ad3ffcf70351b35bc37893e6ab1224c96", @"/Views/_ViewImports.cshtml")]
    public class Views_Users_ConfirmEmail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Users\ConfirmEmail.cshtml"
  
    ViewData["Title"] = "Email Confirmed";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"alert alert-icon alert-info\" role=\"alert\">\r\n    <i class=\"fe fe-check mr-2\"></i> Your account has been activated! <a");
            BeginWriteAttribute("href", " href=\"", 183, "\"", 218, 1);
#nullable restore
#line 7 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Users\ConfirmEmail.cshtml"
WriteAttributeValue("", 190, Url.Action("Login","Users"), 190, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("> Login</a>\r\n</div>");
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
