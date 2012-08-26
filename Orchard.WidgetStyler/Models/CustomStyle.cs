using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Orchard.WidgetStyler.Models
{
    public class CustomStyle:ContentPart<CustomStyleRecord>
     {

        public  string CssStyle {
            get {  return  Record.CssStyle; }
            set { Record.CssStyle=value; }
        }
    }
}