using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLConverter
{
    public static class myCssPraser
    {
        public static string fontSizeConverter(string value,string styleValue)
        {
            double size = double.Parse(value);
            if(size == 0) return "1" + "px";
            string tag = styleValue.Replace(value, "");
            switch (tag)
            {
                case "pt":
                case "in":
                case "cm":
                case "px":
                    return value + tag;
                case "mm":
                    size *= 3.779528;
                    return size.ToString() + "px";
                case "pc":
                    size *= 13;
                    return size.ToString() + "px";
                case "em":
                    size *= 13;
                    return size.ToString() + "px";
                case "ex":
                    return "10" + "px";
                case "ch":
                    size *= 32;
                    return size.ToString() + "px";
                case "rem":
                    size *= 13;
                    return size.ToString() + "px";
                case "vw":
                    size *= 13;
                    return size.ToString() + "px";
                case "vh":
                    size *= 13;
                    return size.ToString() + "px";
                case "%":
                    return size.ToString() + "px";
                case "vmin":
                    return "10" + "px";
                case "vmax":
                    return "10" + "px";
            }
            return "";
        }
    }
}
