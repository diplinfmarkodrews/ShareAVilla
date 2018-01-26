using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Text;
using Newtonsoft;

namespace ShareAVilla
{
    public static class HtmlCustomHelper
    {
        public static MvcHtmlString AntiForgeryTokenForAjaxPost(this HtmlHelper helper)
        {
            var antiForgeryInputTag = helper.AntiForgeryToken().ToString();
            // Above gets the following: <input name="__RequestVerificationToken" type="hidden" value="PnQE7R0MIBBAzC7SqtVvwrJpGbRvPgzWHo5dSyoSaZoabRjf9pCyzjujYBU_qKDJmwIOiPRDwBV1TNVdXFVgzAvN9_l2yt9-nf4Owif0qIDz7WRAmydVPIm6_pmJAI--wvvFQO7g0VvoFArFtAR2v6Ch1wmXCZ89v0-lNOGZLZc1" />
            var removedStart = antiForgeryInputTag.Replace(@"<input name=""__RequestVerificationToken"" type=""hidden"" value=""", "");
            var tokenValue = removedStart.Replace(@""" />", "");
            if (antiForgeryInputTag == removedStart || removedStart == tokenValue)
                throw new InvalidOperationException("Oops! The Html.AntiForgeryToken() method seems to return something I did not expect.");
            return new MvcHtmlString(tokenValue);
        }
        public static IHtmlString ToJson(this HtmlHelper helper, object obj)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            return helper.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings));
        }
        public static IHtmlString Image(this HtmlHelper html, byte[] image)
        {
            var img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));
            return new HtmlString("<img src='" + img + "' />");
        }
        public static IHtmlString Image(this HtmlHelper html, byte[] image, string htmlClass)
        {
            var img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));
            return new HtmlString("<img src='" + img + "' class='" +htmlClass+ "' />");
        }
        //public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
        //    this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression,
        //    IEnumerable<SelectListItem> listOfValues)
        //{
        //    var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        //    var sb = new StringBuilder();

        //    if (listOfValues != null)
        //    {
        //        // Create a radio button for each item in the list 
        //        foreach (SelectListItem item in listOfValues)
        //        {
        //            // Generate an id to be given to the radio button field 
        //            var id = string.Format("{0}_{1}", metaData.PropertyName, item.Value);

        //            // Create and populate a radio button using the existing html helpers 
        //            var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));

        //            var radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id }).ToHtmlString();

        //            if (item.Selected)
        //            {
        //                radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id, @checked = "checked", }).ToHtmlString();
        //            }


        //            // Create the html string that will be returned to the client 
        //            // e.g. <input data-val="true" data-val-required="You must select an option" id="TestRadio_1" name="TestRadio" type="radio" value="1" /><label for="TestRadio_1">Line1</label> 
        //            sb.AppendFormat("<div class=\"RadioButton\">{0}{1}</div>", radio, label);
        //        }
        //    }

        //    return MvcHtmlString.Create(sb.ToString());
        //}

    }
}