using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotwireTurbo {
    public class TurboStreamRemoveTagHelper : TurboStreamTagHelper {
        public TurboStreamRemoveTagHelper() : base("remove") { }
    }

    public class TurboStreamReplaceTagHelper : TurboStreamTagHelper {
        public TurboStreamReplaceTagHelper() : base("replace") { }
    }

    public class TurboStreamPrependTagHelper : TurboStreamTagHelper {
        public TurboStreamPrependTagHelper() : base("prepend") { }
    }

    public abstract class TurboStreamTagHelper : TagHelper {
        public object Target { get; set; }
        private string action;

        public TurboStreamTagHelper(string action) {
            this.action = action;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            var target = convertToDomId(Target);
            output.Attributes.SetAttribute("action", action);
            output.Attributes.SetAttribute("target", target);
            var pre = action == "remove"
                ? ""
                : "<template>";
            output.PreContent.SetHtmlContent(pre);
            var post = action == "remove"
                ? ""
                : "</template>";
            output.PostContent.SetHtmlContent(post);
            if (action != "remove") {
                var content = await output.GetChildContentAsync();
                output.Content.SetHtmlContent(content.GetContent());
            }
        }

        private string convertToDomId(object target) {
            return target switch {
                IHasDomId hasDomId => hasDomId.ToDomId(),
                string @string => @string,
                _ => throw new InvalidEnumArgumentException("target must be of type IHasDomId or string")
            };
        }
    }

    public class TurboFrameTagHelper : TagHelper {
        public object Id { get; set; }
        public string Target { get; set; }
        public string Src { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            var id = convertToDomId(Id);
            output.Attributes.SetAttribute("id", id);
            if (!string.IsNullOrEmpty(Target)) output.Attributes.SetAttribute("target", Target);
            if (!string.IsNullOrEmpty(Src)) output.Attributes.SetAttribute("src", Src);
            output.Content.SetHtmlContent((await output.GetChildContentAsync()).GetContent());
        }

        private string convertToDomId(object target) {
            return target switch {
                IHasDomId hasDomId => hasDomId.ToDomId(),
                string @string => @string,
                _ => throw new InvalidEnumArgumentException("target must be of type IHasDomId or string")
            };
        }
    }
}