#pragma checksum "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Errors\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "514f8fa6dd0306f005077bd36cfe75d69dc42cb5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Errors_Index), @"mvc.1.0.view", @"/Views/Errors/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"514f8fa6dd0306f005077bd36cfe75d69dc42cb5", @"/Views/Errors/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2eb0a75ad3ffcf70351b35bc37893e6ab1224c96", @"/Views/_ViewImports.cshtml")]
    public class Views_Errors_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Errors\Index.cshtml"
  
    ViewData["Title"] = "Error";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"container text-center\">\r\n    <div class=\"display-1 text-muted mt-9\"><i class=\"si si-exclamation\"></i> ");
#nullable restore
#line 6 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Errors\Index.cshtml"
                                                                        Write(TempData["Code"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n    <div class=\"display-4 text-muted mb-5\"><i class=\"si si-exclamation\"></i> ");
#nullable restore
#line 7 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaTabler\Views\Errors\Index.cshtml"
                                                                        Write(TempData["Message"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>
    <h1 class=""h2 mb-3"">Oops.. You just found an error page..</h1>
    <p class=""h4 text-muted font-weight-normal mb-7"">We are sorry but our service is currently not available&hellip;</p>
    <a class=""btn btn-primary"" href=""javascript:history.back()"">
        <i class=""fe fe-arrow-left mr-2""></i>Go back
    </a>
</div>
");
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
