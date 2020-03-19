#pragma checksum "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\CoreUI\base-popovers.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7e0c9ff0e001eeae7c2d7fac6b9f6d425803dff6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CoreUI_base_popovers), @"mvc.1.0.view", @"/Views/CoreUI/base-popovers.cshtml")]
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
using FandaCoreUI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\_ViewImports.cshtml"
using FandaCoreUI.Extensions;

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
using Fanda.Shared;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7e0c9ff0e001eeae7c2d7fac6b9f6d425803dff6", @"/Views/CoreUI/base-popovers.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f17fc0d007cc7338106c312cff473596613e7c1a", @"/Views/_ViewImports.cshtml")]
    public class Views_CoreUI_base_popovers : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/popovers.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<div class=""card"">
  <div class=""card-header"">
    <i class=""fa fa-align-justify""></i> Popovers
    <div class=""card-header-actions"">
      <a href=""http://coreui.io/docs/components/bootstrap-popover/popovers.html"" class=""card-header-action"" target=""_blank"">
        <small class=""text-muted"">docs</small>
      </a>
    </div>
  </div>
  <div class=""card-body"">
    <button type=""button"" class=""btn btn-lg btn-danger"" data-toggle=""popover"" title=""Popover title"" data-content=""And here's some amazing content. It's very engaging. Right?"">
      Click to toggle popover
    </button>
    <hr />
    <a tabindex=""0"" class=""btn btn-lg btn-danger"" role=""button"" data-toggle=""popover"" data-trigger=""focus"" title=""Dismissible popover"" data-content=""And here's some amazing content. It's very engaging. Right?"">
      Dismissible popover
    </a>
  </div>
</div>
<div class=""card"">
  <div class=""card-header"">
    <i class=""fa fa-align-justify""></i> Popovers
    <small>directions</small>
  </div>
  <div class=""card-body"">
    <bu");
            WriteLiteral(@"tton type=""button"" class=""btn btn-secondary"" data-container=""body"" data-toggle=""popover"" data-placement=""top"" data-content=""Vivamus sagittis lacus vel augue laoreet rutrum faucibus."">
      Popover on top
    </button>

    <button type=""button"" class=""btn btn-secondary"" data-container=""body"" data-toggle=""popover"" data-placement=""right"" data-content=""Vivamus sagittis lacus vel augue laoreet rutrum faucibus."">
      Popover on right
    </button>

    <button type=""button"" class=""btn btn-secondary"" data-container=""body"" data-toggle=""popover"" data-placement=""bottom"" data-content=""Vivamus sagittis lacus vel augue laoreet rutrum faucibus."">
      Popover on bottom
    </button>

    <button type=""button"" class=""btn btn-secondary"" data-container=""body"" data-toggle=""popover"" data-placement=""left"" data-content=""Vivamus sagittis lacus vel augue laoreet rutrum faucibus."">
      Popover on left
    </button>
  </div>
</div>

");
            DefineSection("Scripts", async() => {
                WriteLiteral("\n  ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7e0c9ff0e001eeae7c2d7fac6b9f6d425803dff66055", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\n");
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
