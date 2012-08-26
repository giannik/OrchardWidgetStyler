using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.WidgetStyler.Models;
using Orchard.Environment;
using Orchard.Environment.Extensions;
namespace Orchard.WidgetStyler.Handlers
{
    public class CustomStyleHandler:ContentHandler
        {

        public CustomStyleHandler(IRepository<CustomStyleRecord> repository) {

            Filters.Add(StorageFilter.For(repository));
            
        }


        protected override void BuildDisplayShape(BuildDisplayContext context)
        {
            if (context.ContentItem.TypeDefinition.Settings.ContainsKey("Stereotype"))
            {
                

                  if (context.ContentItem.TypeDefinition.Settings["Stereotype"] == "Widget" && context.ContentItem.Has(typeof(CustomStyle))) {
                    var cssPart = context.ContentItem.As<CustomStyle>();
                    var customcssstyle = cssPart.CssStyle;
                    if (!string.IsNullOrWhiteSpace(customcssstyle)) {

                        context.Shape.CustomCssStyle = customcssstyle;
                    }
 
                }

            base.BuildDisplayShape(context);

            }
                
                
                
                 
              
        }

    }
}