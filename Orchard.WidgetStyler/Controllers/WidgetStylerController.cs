using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.WidgetStyler.Models;

namespace Orchard.WidgetStyler.Controllers
{
    public class WidgetStylerController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly IRepository<CustomStyleRecord> _repo;

        public WidgetStylerController(IContentManager contentManager, IRepository<CustomStyleRecord> repo)
        {
            _contentManager = contentManager;
            _repo = repo;
        }

        public ActionResult ShowJqueryPopup(int widgetid, string widgetidentifier) {



            var widgetTitle = _contentManager.Get<Orchard.Widgets.Models.WidgetPart>(widgetid).Title;

            ViewBag.widgetid = widgetid;
            ViewBag.widgetidentifier = widgetidentifier;
            ViewBag.widgetTitle = widgetTitle;
            return PartialView();
        }
       
        private Dictionary<string, string> ParseStringTocssCollection(string css)
        {


            var innercol = new Dictionary<string, string>();
            if (String.IsNullOrEmpty(css)==false) {
          
            String[] dd = css.Split(Convert.ToChar(";"));

            var clean = dd.Where(x => String.IsNullOrWhiteSpace(x) == false);

                foreach (var s in clean)
                {
                    //parse inner string

                    string[] propnameVal = s.Split(Convert.ToChar(":"));

                    innercol.Add(propnameVal[0], propnameVal[1]);

                }
            }
            return innercol;
        }



        private string CssCollectionTostring(Dictionary<string, string> csscol)
        {
            var sb = new StringBuilder();

            foreach (var item in csscol)
            {
                sb.Append(string.Format("{0}:{1};", item.Key, item.Value));
            }



            return sb.ToString();

        }


        public ActionResult BackGroundStyling(int widgetid)
        {
            //get style

            var item = _contentManager.Get<CustomStyle>(widgetid);

            if (item != null)
            {

                string css = item.CssStyle;


                if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
 

                //parse
                //strip out the px symbol
                css = css.Replace("px", "");
                var col = ParseStringTocssCollection(css);
                ViewBag.widgetid = widgetid;


                var mdl = new StandardSettings();
                if (col.ContainsKey("background-color")) mdl.BackGroundColor =col["background-color"];
                
     

                return PartialView(mdl);


            }


            return null;
        }

        [HttpPost]
        public JsonResult SaveBackGroundStyling(int widgetidbackground, StandardSettings mdl)
        {

            if (ModelState.IsValid)
            {
                var item = _contentManager.Get<CustomStyle>(widgetidbackground);
                if (item != null)
                {
                    string css = item.CssStyle;
                    if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
                    //parse
                    var col = ParseStringTocssCollection(css);
                    //clear any previous instanses
                    if (col.ContainsKey("background-color")) col.Remove("background-color");
                    // add css styles from model
                    col.Add("background-color", mdl.BackGroundColor);


                    //serialize back 
                    item.CssStyle = CssCollectionTostring(col);


                    _repo.Update(item.Record);

                    return Json(new { s = "Saved settings!" });
                }
            }
            else
            {
                //model state is not valid
                return Json(new { s = PrepareErrMsg(ModelState) });

            }
 
            return null;
        }

        public ActionResult SpacingStyling(int widgetid)
        {
            var item = _contentManager.Get<CustomStyle>(widgetid);

            if (item != null)
            {

                string css = item.CssStyle;
                //parse
                if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
                //strip out the px symbol
                css = css.Replace("px", "");
                var col = ParseStringTocssCollection(css);
                ViewBag.widgetid = widgetid;
              

                var mdl = new SpacingSettings();

                //margins
                if (col.ContainsKey("margin"))
                {
                    //parse 

                    string shadowvals = col["margin"];

                    string[] shadowvalsArray = shadowvals.Split(Convert.ToChar(" "));

                    if (shadowvalsArray.Count()==1) {
                        mdl.MarginSameForAll = true;
                        mdl.MarginTop = Convert.ToInt32(shadowvalsArray[0]);


                    }
                    else
                        {
                        mdl.MarginSameForAll = false;
                        mdl.MarginTop = Convert.ToInt32(shadowvalsArray[0]);
                        mdl.MarginRight = Convert.ToInt32(shadowvalsArray[1]);
                        mdl.MarginBottom = Convert.ToInt32(shadowvalsArray[2]);
                        mdl.MarginLeft = Convert.ToInt32(shadowvalsArray[3]);
                        }

                   }

                //paddings
                if (col.ContainsKey("padding"))
                {
                    //parse 

                    string shadowvals = col["padding"];

                    string[] paddingsvalarray  = shadowvals.Split(Convert.ToChar(" "));

                    if (paddingsvalarray.Count() == 1) {
                        mdl.PaddingSameForAll = true;
                        mdl.PaddingTop = Convert.ToInt32(paddingsvalarray[0]);

                    }
                    else

                        {
                            mdl.PaddingSameForAll = false;
                            mdl.PaddingTop = Convert.ToInt32(paddingsvalarray[0]);
                            mdl.PaddingRight = Convert.ToInt32(paddingsvalarray[1]);
                            mdl.PaddingBottom = Convert.ToInt32(paddingsvalarray[2]);
                            mdl.PaddingLeft = Convert.ToInt32(paddingsvalarray[3]);
                        }



                     

                }


                return PartialView(mdl);
            }

            return null;
        }


        [HttpPost]
        public ActionResult SaveSpacingStyling(int widgetidspacing, SpacingSettings mdl)
        {

      if (ModelState.IsValid) {
        
            var item = _contentManager.Get<CustomStyle>(widgetidspacing);
            if (item != null)
            {
                string css = item.CssStyle;
                if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
                //parse
                var col = ParseStringTocssCollection(css);
                //properties (margin: 50px 50px 50px 5px;)


                //clear any previous margin instanses
                if (col.ContainsKey("margin")) col.Remove("margin");
                //build padding rule
                if (mdl.MarginSameForAll==true) 
                    {
                    string marginrule = string.Format("{0}px", mdl.MarginTop);
                    // add css styles from model
                    col.Add("margin", marginrule);
                    }

                else {
                    string marginrule = string.Format("{0}px {1}px {2}px {3}px", mdl.MarginTop, mdl.MarginRight, mdl.MarginBottom, mdl.MarginLeft);
                    // add css styles from model
                    col.Add("margin", marginrule);
                    }


                //clear any previous padding instanses
                if (col.ContainsKey("padding")) col.Remove("padding");
                //build padding rule
                if (mdl.PaddingSameForAll == true)
                {
                    string paddingrule = string.Format("{0}px", mdl.PaddingTop);
                    // add css styles from model
                    col.Add("padding", paddingrule);
                }

                else
                {
                    string paddingrule = string.Format("{0}px {1}px {2}px {3}px", mdl.PaddingTop, mdl.PaddingRight, mdl.PaddingBottom, mdl.PaddingLeft);
                    // add css styles from model
                    col.Add("padding", paddingrule);
                }


                //serialize back
                item.CssStyle = CssCollectionTostring(col);


                _repo.Update(item.Record);

                return Json(new { s = "Saved settings!" });
            }

      }

          else
          {
             //model state is not valid
              return Json(new { s = PrepareErrMsg(ModelState) });
          
        

          }
            return null;

        }


        public ActionResult BordersStyling(int widgetid)
        {
            var item = _contentManager.Get<CustomStyle>(widgetid);

            if (item != null)
            {

                string css = item.CssStyle;
                //parse
                if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
                //strip out the px symbol
                css = css.Replace("px", "");
                var col = ParseStringTocssCollection(css);
                ViewBag.widgetid = widgetid;


                var mdl = new BorderSettings();

                //border-width
                if (col.ContainsKey("border-width"))
                {
                    //parse 

                    string borderwidthvals = col["border-width"];

                    string[] borderwidthvalsArray = borderwidthvals.Split(Convert.ToChar(" "));

                    if (borderwidthvalsArray.Count() == 1)
                    {
                        mdl.WidthSameForAll = true;
                        mdl.WidthTop = Convert.ToInt32(borderwidthvalsArray[0]);


                    }
                    else
                    {
                        mdl.WidthSameForAll = false;
                        mdl.WidthTop = Convert.ToInt32(borderwidthvalsArray[0]);
                        mdl.WidthRight = Convert.ToInt32(borderwidthvalsArray[1]);
                        mdl.WidthBottom = Convert.ToInt32(borderwidthvalsArray[2]);
                        mdl.WidthLeft = Convert.ToInt32(borderwidthvalsArray[3]);
                    }

                }

                //border-color
                if (col.ContainsKey("border-color"))
                {
                    //parse 

                    string bordercolorvals = col["border-color"];

                    string[] bordercolorvalsArray = bordercolorvals.Split(Convert.ToChar(" "));

                    if (bordercolorvalsArray.Count() == 1)
                    {
                        mdl.ColorSameForAll = true;
                        mdl.ColorTop =  bordercolorvalsArray[0] ;


                    }
                    else
                    {
                        mdl.ColorSameForAll = false;
                        mdl.ColorTop =  bordercolorvalsArray[0];
                        mdl.ColorRight =  bordercolorvalsArray[1];
                        mdl.ColorBottom =  bordercolorvalsArray[2];
                        mdl.ColorLeft =  bordercolorvalsArray[3];
                    }

                }

                //border-style
                if (col.ContainsKey("border-style"))
                {
                    //parse 

                    string borderstylevals = col["border-style"];

                    string[] borderstylevalsArray = borderstylevals.Split(Convert.ToChar(" "));

                    if (borderstylevalsArray.Count() == 1)
                    {
                        mdl.StyleSameForAll = true;
                        mdl.StyleTop =  borderstylevalsArray[0] ;


                    }
                    else
                    {
                        mdl.StyleSameForAll = false;
                        mdl.StyleTop =  borderstylevalsArray[0] ;
                        mdl.StyleRight =  borderstylevalsArray[1] ;
                        mdl.StyleBottom =  borderstylevalsArray[2] ;
                        mdl.StyleLeft = borderstylevalsArray[3] ;
                    }

                }


                //border-radius
                if (col.ContainsKey("border-radius"))
                {
                    //parse 

                    string borderradiusvals = col["border-radius"];

                    string[] borderradiusvalsArray = borderradiusvals.Split(Convert.ToChar(" "));

                    if (borderradiusvalsArray.Count() == 1)
                    {
                        mdl.RadiusSameForAll = true;
                        mdl.RadiusTop = Convert.ToInt32(borderradiusvalsArray[0]);


                    }
                    else
                    {
                        mdl.RadiusSameForAll = false;
                        mdl.RadiusTop = Convert.ToInt32(borderradiusvalsArray[0]);
                        mdl.RadiusRight = Convert.ToInt32(borderradiusvalsArray[1]);
                        mdl.RadiusBottom = Convert.ToInt32(borderradiusvalsArray[2]);
                        mdl.RadiusLeft = Convert.ToInt32(borderradiusvalsArray[3]);
                    }

                }

                return PartialView(mdl);
            }

            return null;
        }

          [HttpPost]
        public ActionResult SaveBorderStyling(int widgetidborders, BorderSettings mdl)
        {

            if (ModelState.IsValid)
            {
                var item = _contentManager.Get<CustomStyle>(widgetidborders);
                if (item != null)
                {
                    string css = item.CssStyle;
                    if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
                    //parse
                    var col = ParseStringTocssCollection(css);
                    //properties (border: 50px 50px 50px 5px;)

                    //clear any previous border-width instanses
                    if (col.ContainsKey("border-width")) col.Remove("border-width");
                    //build border-width rule
                    if (mdl.WidthSameForAll == true)
                    {
                        string borderwidthrule = string.Format("{0}px", mdl.WidthTop);
                        // add css styles from model
                        col.Add("border-width", borderwidthrule);
                    }

                    else
                    {
                        string borderwidthrule = string.Format("{0}px {1}px {2}px {3}px", mdl.WidthTop, mdl.WidthRight, mdl.WidthBottom, mdl.WidthLeft);
                        // add css styles from model
                        col.Add("border-width", borderwidthrule);
                    }



                    //clear any previous border-color instanses
                    if (col.ContainsKey("border-color")) col.Remove("border-color");
                    //build border-width rule
                    if (mdl.ColorSameForAll == true)
                    {
                        string bordercolorrule = string.Format("{0}", mdl.ColorTop);
                        // add css styles from model
                        col.Add("border-color", bordercolorrule);
                    }

                    else
                    {
                        string bordercolorrule = string.Format("{0} {1} {2} {3}", mdl.ColorTop, mdl.ColorRight, mdl.ColorBottom, mdl.ColorLeft);
                        // add css styles from model
                        col.Add("border-color", bordercolorrule);
                    }


                    //clear any previous border-style instanses
                    if (col.ContainsKey("border-style")) col.Remove("border-style");
                    //build border-width rule
                    if (mdl.StyleSameForAll == true)
                    {
                        string borderstylerule = string.Format("{0}", mdl.StyleTop);
                        // add css styles from model
                        col.Add("border-style", borderstylerule);
                    }

                    else
                    {
                        string borderstylerule = string.Format("{0} {1} {2} {3}", mdl.StyleTop, mdl.StyleRight, mdl.StyleBottom, mdl.StyleLeft);
                        // add css styles from model
                        col.Add("border-style", borderstylerule);
                    }


                    //clear any previous border-radius instanses
                    if (col.ContainsKey("border-radius")) col.Remove("border-radius");
                    //build border-width rule
                    if (mdl.RadiusSameForAll == true)
                    {
                        string borderradiusrule = string.Format("{0}px", mdl.RadiusTop);
                        // add css styles from model
                        col.Add("border-radius", borderradiusrule);
                    }

                    else
                    {
                        string borderradiusrule = string.Format("{0}px {1}px {2}px {3}px", mdl.RadiusTop, mdl.RadiusRight, mdl.RadiusBottom, mdl.RadiusLeft);
                        // add css styles from model
                        col.Add("border-radius", borderradiusrule);
                    }

                    //serialize back
                    item.CssStyle = CssCollectionTostring(col);


                    _repo.Update(item.Record);

                    return Json(new { s = "Saved settings!" });
                }
            }
            else
            {
                //model state is not valid
                return Json(new { s = PrepareErrMsg(ModelState) });

            }
 
            return null;


        }

          public ActionResult ShadowStyling(int widgetid)
        {
            var item = _contentManager.Get<CustomStyle>(widgetid);

            if (item != null)
            {
               
                string css = item.CssStyle;
                //parse
                if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
                //strip out the px symbol
                css = css.Replace("px", "");
                var col = ParseStringTocssCollection(css);
                ViewBag.widgetid = widgetid;


                var mdl = new ShadowSettings();
             


                if (col.ContainsKey("box-shadow")) {
                    //parse 
                
                    string shadowvals = col["box-shadow"];

                    string[] shadowvalsArray = shadowvals.Split(Convert.ToChar(" "));



                    mdl.HorizontalOffset = Convert.ToInt32(shadowvalsArray[0]);
                    mdl.VerticalOffset= Convert.ToInt32(shadowvalsArray[1]);
                    mdl.Blur= Convert.ToInt32(shadowvalsArray[2]);
                    mdl.Size  = Convert.ToInt32(shadowvalsArray[3]);
                    mdl.ShadowColor= shadowvalsArray[4];


                }
                    
                    
                 
                return PartialView(mdl);
            }

            return null;
        }

          [HttpPost]
          public ActionResult SaveShadowStyling(int widgetidshadows, ShadowSettings mdl)
        {

            if (ModelState.IsValid)
            {
                var item = _contentManager.Get<CustomStyle>(widgetidshadows);
                if (item != null)
                {
                    string css = item.CssStyle;
                    if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
                    //parse
                    var col = ParseStringTocssCollection(css);

                    //properties (box-shadow: 50px 50px 50px 5px black;)

                    //clear any previous instanses
                    if (col.ContainsKey("box-shadow")) col.Remove("box-shadow");
                    //build shadow rule 
                    string shadowrule = string.Format("{0}px {1}px {2}px {3}px {4}", mdl.HorizontalOffset, mdl.VerticalOffset, mdl.Blur, mdl.Size, mdl.ShadowColor);
                    // add css styles from model
                    col.Add("box-shadow", shadowrule);

                    //serialize back 
                    item.CssStyle = CssCollectionTostring(col);

                    _repo.Update(item.Record);

                    return Json(new { s = "Saved settings!" });

                }
            }
            else
            {
                //model state is not valid
                return Json(new { s = PrepareErrMsg(ModelState) });

            }
 
              return null;
          }

        
        public ActionResult TextsStyling(int widgetid)
        {
             var item = _contentManager.Get<CustomStyle>(widgetid);

            if (item != null)
            {

                string css = item.CssStyle;
                //parse
                if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
                //strip out the px symbol
                css = css.Replace("px", "");


                var col = ParseStringTocssCollection(css);
                ViewBag.widgetid = widgetid;


                var mdl = new TextSettings();
                if (col.ContainsKey("color")) mdl.FontColor=col["color"];
                if (col.ContainsKey("text-align")) mdl.Alignment = col["text-align"];
                if (col.ContainsKey("font-family")) mdl.Font = col["font-family"];
 
                return PartialView(mdl);
            }

            return null;
        }

        [HttpPost]
        public ActionResult SaveTextsStyling(int widgetidtexts, TextSettings mdl)
        {

            if (ModelState.IsValid)
            {
                var item = _contentManager.Get<CustomStyle>(widgetidtexts);
                if (item != null)
                {
                    string css = item.CssStyle;
                    //parse
                    if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
                    var col = ParseStringTocssCollection(css);

                    //clear any previous instanses
                    if (col.ContainsKey("color")) col.Remove("color");
                    if (col.ContainsKey("text-align")) col.Remove("text-align");
                    if (col.ContainsKey("font-family")) col.Remove("font-family");
                    // add css styles from model
                    col.Add("color", mdl.FontColor);
                    col.Add("text-align", mdl.Alignment);
                    col.Add("font-family", mdl.Font);





                    //serialize back 
                    item.CssStyle = CssCollectionTostring(col);

                    _repo.Update(item.Record);

                    return Json(new { s = "Saved settings!" });

                }
            }
            else
            {
                //model state is not valid
                return Json(new { s = PrepareErrMsg(ModelState) });

            }
 
            return null;
        }


        public ActionResult AdvancedStyling(int widgetid)
        {
            var item = _contentManager.Get<CustomStyle>(widgetid);

            if (item != null)
            {

                string css = item.CssStyle;
                //parse
                if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
                //strip out the px symbol
                css = css.Replace("px", "");
                var col = ParseStringTocssCollection(css);
                ViewBag.widgetid = widgetid;


                var mdl = new AdvancedSettings();


                return PartialView(mdl);
            }

            return null;
        }

        [HttpPost]
        public ActionResult SaveAdvancedStyling(int widgetidadvanced, AdvancedSettings mdl)
        {

            if (ModelState.IsValid)
            {
                var item = _contentManager.Get<CustomStyle>(widgetidadvanced);
                if (item != null)
                {
                    string css = item.CssStyle;
                    if (string.IsNullOrWhiteSpace(css)) css = string.Empty;
                    //parse
                    var col = ParseStringTocssCollection(css);
                    //set properties

                    //serialize back 
                    item.CssStyle = CssCollectionTostring(col);

                    _repo.Update(item.Record);

                    return Json(new { s = "Saved settings!" });

                }
            }
            else
            {
                //model state is not valid
                return Json(new { s = PrepareErrMsg(ModelState) });

            }


         


            return null;
        }

        [HttpPost]
        public ActionResult ClearCustomStyling(int widgetidclearstyling)
        {
            var item = _contentManager.Get<CustomStyle>(widgetidclearstyling);
            if (item != null) {
                
                
                item.CssStyle = string.Empty;

               
                _repo.Update(item.Record);

                return Json(new { s = "Custom styling removed!" + System.Environment.NewLine + "Refresh page to view update." });

            }


            return null;
        }





        private string PrepareErrMsg(ModelStateDictionary mdl) {
            List<ModelErrorCollection> errors = mdl.Select(x => x.Value.Errors).ToList();
            String msg = string.Empty;

            foreach (ModelErrorCollection error in errors)
            {
                foreach (ModelError modelError in error)
                {
                    msg += msg + modelError.ErrorMessage + " ";
                }

            }
            return msg;
        }


    }


}