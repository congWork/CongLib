using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CongLib.MvcHelpers
{
    public static class AngularHelperExtension{
        public static AngularHelper<TModel> Angular<TModel>(this HtmlHelper<TModel> helper){
            return new AngularHelper<TModel>(helper);
        }
            
    }
    public class AngularHelper<TModel>
    {
        private readonly HtmlHelper<TModel> _htmlHelper;
        public AngularHelper(HtmlHelper<TModel> helper)
        {
            _htmlHelper = helper;
        }
        public AngularModelHelper<TModel> ModelFor(string expressionPrefix)
        {
            return new AngularModelHelper<TModel>(_htmlHelper, expressionPrefix);
        }
    }
}