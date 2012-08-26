using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.UI.Notify;
using Orchard.WidgetStyler.Models;

namespace Orchard.WidgetStyler.Drivers
{
    public class CustomStyleDriver:ContentPartDriver<CustomStyle> {
        private readonly INotifier _notifier;
        private const string TemplateName = "Parts/CustomStyle";
        protected override string Prefix
        {
            get { return "customer_css_style"; }
        }
        public Localizer T { get; set; }


        public CustomStyleDriver(INotifier notifier)
        {
            _notifier = notifier;
            T = NullLocalizer.Instance;  
        }

        protected override DriverResult Display(CustomStyle part, string displayType, dynamic shapeHelper)
        {
            return new DriverResult();
        }

        protected override DriverResult Editor(CustomStyle part, dynamic shapeHelper)
        {
            return ContentShape("Parts_CustomStyle_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(CustomStyle part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (!updater.TryUpdateModel(part, Prefix, null, null))
            {
                _notifier.Error(T("Error during CustomCss update."));
            }
            return Editor(part, shapeHelper);
        }

    }
}