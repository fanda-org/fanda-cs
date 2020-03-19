#pragma checksum "D:\Development\Projects\dotnet\Fanda\src\Fanda\FandaCoreUI\Views\CoreUI\base-pagination.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "73b0ca59aee9893aa065063fe9fa057e7e137de5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CoreUI_base_pagination), @"mvc.1.0.view", @"/Views/CoreUI/base-pagination.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"73b0ca59aee9893aa065063fe9fa057e7e137de5", @"/Views/CoreUI/base-pagination.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f17fc0d007cc7338106c312cff473596613e7c1a", @"/Views/_ViewImports.cshtml")]
    public class Views_CoreUI_base_pagination : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<div class=""card"">
  <div class=""card-header"">
    <i class=""fa fa-align-justify""></i> Pagination
    <div class=""card-header-actions"">
      <a href=""http://coreui.io/docs/components/bootstrap-pagination/"" class=""card-header-action"" target=""_blank"">
        <small class=""text-muted"">docs</small>
      </a>
    </div>
  </div>
  <div class=""card-body"">
    <nav aria-label=""Page navigation example"">
      <ul class=""pagination"">
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">Previous</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">1</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">2</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">3</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">Next</a>
        </li>
      </ul>
    </nav>
  </div>
</div>
<div class=""card"">
  <div class=""card-header"">
    <i class=""fa fa-align-justify""></i> P");
            WriteLiteral(@"agination
    <small>with icons</small>
  </div>
  <div class=""card-body"">
    <nav aria-label=""Page navigation example"">
      <ul class=""pagination"">
        <li class=""page-item"">
          <a class=""page-link"" href=""#"" aria-label=""Previous"">
            <span aria-hidden=""true"">&laquo;</span>
            <span class=""sr-only"">Previous</span>
          </a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">1</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">2</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">3</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"" aria-label=""Next"">
            <span aria-hidden=""true"">&raquo;</span>
            <span class=""sr-only"">Next</span>
          </a>
        </li>
      </ul>
    </nav>
  </div>
</div>
<div class=""card"">
  <div class=""card-header"">
    <i class=""fa fa-align-justify""></i> Pagination
    <small>disabled ");
            WriteLiteral(@"and active</small>
  </div>
  <div class=""card-body"">
    <nav aria-label=""..."">
      <ul class=""pagination"">
        <li class=""page-item disabled"">
          <a class=""page-link"" href=""#"" tabindex=""-1"">Previous</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">1</a>
        </li>
        <li class=""page-item active"">
          <a class=""page-link"" href=""#"">
            2
            <span class=""sr-only"">(current)</span>
          </a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">3</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">Next</a>
        </li>
      </ul>
    </nav>
    <hr />
    <nav aria-label=""..."">
      <ul class=""pagination"">
        <li class=""page-item disabled"">
          <span class=""page-link"">Previous</span>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">1</a>
        </li>
        <li class=""page-item active"">
          <span cla");
            WriteLiteral(@"ss=""page-link"">
            2
            <span class=""sr-only"">(current)</span>
          </span>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">3</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">Next</a>
        </li>
      </ul>
    </nav>
  </div>
</div>
<div class=""card"">
  <div class=""card-header"">
    <i class=""fa fa-align-justify""></i> Pagination
    <small>sizing</small>
  </div>
  <div class=""card-body"">
    <nav aria-label=""..."">
      <ul class=""pagination pagination-lg"">
        <li class=""page-item disabled"">
          <a class=""page-link"" href=""#"" tabindex=""-1"">Previous</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">1</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">2</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">3</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" h");
            WriteLiteral(@"ref=""#"">Next</a>
        </li>
      </ul>
    </nav>
    <hr />
    <nav aria-label=""..."">
      <ul class=""pagination pagination-sm"">
        <li class=""page-item disabled"">
          <a class=""page-link"" href=""#"" tabindex=""-1"">Previous</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">1</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">2</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">3</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">Next</a>
        </li>
      </ul>
    </nav>
  </div>
</div>
<div class=""card"">
  <div class=""card-header"">
    <i class=""fa fa-align-justify""></i> Pagination
    <small>alignment</small>
  </div>
  <div class=""card-body"">
    <nav aria-label=""Page navigation example"">
      <ul class=""pagination justify-content-start"">
        <li class=""page-item disabled"">
          <a class=""page-link"" href=""#"" tabindex=""-1"">Previo");
            WriteLiteral(@"us</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">1</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">2</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">3</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">Next</a>
        </li>
      </ul>
    </nav>
    <hr />
    <nav aria-label=""Page navigation example"">
      <ul class=""pagination justify-content-center"">
        <li class=""page-item disabled"">
          <a class=""page-link"" href=""#"" tabindex=""-1"">Previous</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">1</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">2</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">3</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">Next</a>
        </li>
      </ul>
  ");
            WriteLiteral(@"  </nav>
    <hr />
    <nav aria-label=""Page navigation example"">
      <ul class=""pagination justify-content-end"">
        <li class=""page-item disabled"">
          <a class=""page-link"" href=""#"" tabindex=""-1"">Previous</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">1</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">2</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">3</a>
        </li>
        <li class=""page-item"">
          <a class=""page-link"" href=""#"">Next</a>
        </li>
      </ul>
    </nav>
  </div>
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
