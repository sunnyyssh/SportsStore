using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModel;

namespace SportsStore.Infrastructure;

[HtmlTargetElement("div", Attributes = "page-model")]
public sealed class PageLinkTagHelper : TagHelper
{
    private readonly IUrlHelperFactory _urlHelperFactory;
    
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }
    
    public PagingInfo? PageModel { get; set; }
    
    public string? PageAction { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ViewContext is null || PageModel is null)
        {
            return;
        }

        var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
        var result = new TagBuilder("div");
        for (int i = 1; i <= PageModel.TotalPages; i++)
        {
            var tag = new TagBuilder("a");
            tag.Attributes["href"] = urlHelper.Action(new UrlActionContext()
            {
                Action = PageAction,
                Values = new { productPage = i }
            });
            tag.InnerHtml.Append(i.ToString());
            result.InnerHtml.AppendHtml(tag);
        }

        output.Content.AppendHtml(result.InnerHtml);
    }

    public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }
}