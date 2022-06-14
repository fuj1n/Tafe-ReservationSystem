using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ReservationSystem_Server.TagHelpers;

[HtmlTargetElement("a", Attributes = AttrName)]
public class ActivePageTagHelper : TagHelper
{
    private const string AttrName = "cth-active";
    private const string CssOverrideTag = "cth-active-class";
    private const string Href = "href";
    
    private readonly IUrlHelper? _urlHelper;

    public ActivePageTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor contextAccessor)
    {
        if(contextAccessor.ActionContext != null)
            _urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
    }
    
    [HtmlAttributeName(CssOverrideTag)]
    public string? CssOverride { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);

        // Remove attributes for this tag helper
        output.Attributes.RemoveAll(AttrName);
        output.Attributes.RemoveAll(CssOverrideTag);
        
        if (_urlHelper == null)
        {
            return;
        }
        
        if (!output.Attributes.ContainsName(Href))
        {
            return;
        }
        
        output.Attributes.TryGetAttribute(Href, out TagHelperAttribute href);
        
        string activeClass = string.IsNullOrWhiteSpace(CssOverride) ? "active" : CssOverride;

        // ReSharper disable once Mvc.ActionNotResolved - this is not controller code
        string? current = _urlHelper.Action();

        if (href.Value.ToString() == current)
        {
            var builder = new TagBuilder("a");
            builder.Attributes.Add("class", activeClass);
            
            output.MergeAttributes(builder);
        }
    }
}