using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CongLib.Extensions;

namespace CongLib.MvcHelpers
{
    public class AngularModelHelper<TModel>
    {
        protected readonly HtmlHelper Helper;
        private readonly string _expressionPrefix;
        public AngularModelHelper(HtmlHelper helper, string expressionPrefix)
        {
            Helper = helper;
            _expressionPrefix = expressionPrefix;
        }
        public IHtmlString BindingFor<TProp>(Expression<Func<TModel, TProp>> property)
        {
            var expressionText = ExpressionForInternal(property);
            return MvcHtmlString.Create("{{"+expressionText+"}}");
        }
        private string ExpressionForInternal<TProp>(Expression<Func<TModel, TProp>> property)
        {
            var camelCaseName = property.ToCamelCaseName();
            var expression = !string.IsNullOrEmpty(_expressionPrefix) ? _expressionPrefix + "." + camelCaseName : camelCaseName;
            return expression;

        }
        public MvcHtmlString FormGroupFor<TProp>(Expression<Func<TModel, TProp>> property)
        {
            var metaData = ModelMetadata.FromLambdaExpression(property, new ViewDataDictionary<TModel>());
            var name = ExpressionHelper.GetExpressionText(property);
            var expression = ExpressionForInternal(property);

            var divBuilder = new TagBuilder("div");
            divBuilder.AddCssClass("form-group");
            divBuilder.ToString(TagRenderMode.EndTag);

            var labelBuilder = new TagBuilder("label");
            labelBuilder.AddCssClass("control-label");
            labelBuilder.MergeAttribute("for", name);
            string labelText = metaData.DisplayName ?? name;
            labelBuilder.SetInnerText(labelText);
            labelBuilder.ToString(TagRenderMode.EndTag);

            string tagName = metaData.DataTypeName == "MultilineText" ? "textarea" : "input";
            var inputBuilder = new TagBuilder(tagName);
            inputBuilder.AddCssClass("form-control");
            inputBuilder.MergeAttribute("ng-model", expression);
            inputBuilder.MergeAttribute("name", name);


            inputBuilder.MergeAttribute("type", "text");
            string placeholderText = metaData.Watermark ?? (labelText+"...");
            inputBuilder.MergeAttribute("placeholder",placeholderText);
            inputBuilder.ToString(TagRenderMode.SelfClosing);

            divBuilder.InnerHtml += labelBuilder.ToString();
            divBuilder.InnerHtml += inputBuilder.ToString();
            

            return new MvcHtmlString(divBuilder.ToString());
          


        }
        
    }
}