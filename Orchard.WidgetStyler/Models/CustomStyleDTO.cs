using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orchard.WidgetStyler.Models
{
 

    public class SpacingSettings {

        public bool PaddingSameForAll { get; set; }
        public  int PaddingRight { get; set; }
        public int PaddingLeft { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingBottom { get; set; }

        public bool MarginSameForAll { get; set; }
        public int MarginRight { get; set; }
        public int MarginLeft { get; set; }

        public int MarginTop { get; set; }

        public int MarginBottom { get; set; }    
        
        
    }



    public class BorderSettings
    {

        public bool WidthSameForAll { get; set; }
        public int WidthRight { get; set; }
        public int WidthLeft { get; set; }
        public int WidthTop { get; set; }
        public int WidthBottom { get; set; }

        public bool StyleSameForAll { get; set; }
        public String StyleRight { get; set; }
        public String StyleLeft { get; set; }
        public String StyleTop { get; set; }
        public String StyleBottom { get; set; }

        public bool ColorSameForAll { get; set; }
        public String ColorRight { get; set; }
        public String ColorLeft { get; set; }
        public String ColorTop { get; set; }
        public String ColorBottom { get; set; }

        public bool RadiusSameForAll { get; set; }
        public int RadiusRight { get; set; }
        public int RadiusLeft { get; set; }
        public int RadiusTop { get; set; }
        public int RadiusBottom { get; set; }

    }


    public  class ShadowSettings {
        public bool EnableShadow { get; set; }
        public int HorizontalOffset { get; set; }
        public int VerticalOffset { get; set; }
        public int Blur { get; set; }
        public int Size { get; set; }

        private string _color;
        public string ShadowColor {
            get { return _color ?? "black"; }
            set { _color = value; }
        }
    }
    public class TextSettings {
        public string Font { get; set; }
        private string _color;
        public String FontColor {
            get { return  _color?? "black"; }
            set { _color = value; }
        }

        private string _alignment;
        public string Alignment {
            get { return _alignment ?? string.Empty; }
            set { _alignment = value; }
        }
    }

    public class StandardSettings
    {
        private string _backColor;
        public String BackGroundColor
        {
            get { return _backColor ?? "white"; }
            set { _backColor = value; }
        }


       

    }

    public class AdvancedSettings
    {

    }

}