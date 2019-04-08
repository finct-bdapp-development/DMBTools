using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMBTools
{
    public class 
        
        CustomFormatter : IFormatProvider, ICustomFormatter 
    {

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!this.Equals(formatProvider))
                return null;
            if (arg == null) return null;
            string numericString = arg.ToString();
            decimal result = 0;
            if (Decimal.TryParse(numericString, out result))
            {
                return ((Math.Truncate(result * 100)) / 100).ToString("0.00");
            }
            else
            {
                return null;
            }
        }

    }
}
