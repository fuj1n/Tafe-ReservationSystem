using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ReservationSystem_Server.TagHelpers;

[HtmlTargetElement("a", Attributes = AttrName)]
public class ActivePageTagHelper : TagHelper
{
    private const string AttrName = "cth-active";
    private const string CssOverrideTag = "cth-active-class";
    private const string Href = "href";
    
    private readonly IUrlHelper _urlHelper;

    public ActivePageTagHelper(IUrlHelper urlHelper)
    {
        _urlHelper = urlHelper;
    }
    
    [HtmlAttributeName(CssOverrideTag)]
    public string? CssOverride { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);

        if (!output.Attributes.ContainsName(Href))
        {
            return;
        }

        output.Attributes.TryGetAttribute(Href, out TagHelperAttribute href);
        
        string activeClass = string.IsNullOrWhiteSpace(CssOverride) ? "active" : CssOverride;

        output.Attributes.RemoveAll(AttrName);
        output.Attributes.RemoveAll(CssOverrideTag);
        
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