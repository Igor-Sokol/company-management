using CompanyManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagement.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            this.urlHelperFactory = helperFactory ?? throw new ArgumentNullException(nameof(helperFactory));
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageInfo PageInfo { get; set; }
        public string PageAction { get; set; }
        public string ButtonGroupClass { get; set; }
        public string ButtonClass { get; set; }
        public string ButtonSelectedClass { get; set; }
        public string ButtonNormalClass { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;

            IUrlHelper urlHelper = this.urlHelperFactory.GetUrlHelper(ViewContext);

            TagBuilder result = new TagBuilder("div");
            result.AddCssClass(ButtonGroupClass);

            for (int i = 1; i <= PageInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");

                PageUrlValues["page"] = i;
                tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                tag.InnerHtml.AppendHtml(i.ToString());

                tag.AddCssClass(ButtonClass);
                tag.AddCssClass(this.PageInfo.CurrentPage == i ? ButtonSelectedClass : ButtonNormalClass);

                result.InnerHtml.AppendHtml(tag);
            }

            output.Content.AppendHtml(result);
        }
    }
}
