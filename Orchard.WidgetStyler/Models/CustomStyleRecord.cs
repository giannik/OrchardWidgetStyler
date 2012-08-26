using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace Orchard.WidgetStyler.Models
{
    public class CustomStyleRecord:ContentPartRecord    
    {
        public virtual string CssStyle { get; set; }

    }
}