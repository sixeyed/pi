using Pi.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Pi.Wcf
{
    public class PiService : IPiService
    {
        private static readonly int _DefaultDp = 6;

        public string Pi(string dp)
        {
            int.TryParse(dp, out int actualDp);
            if (actualDp < 1)
            {
                actualDp = _DefaultDp;
            }
            var pi = MachinFormula.Calculate(actualDp);
            return pi.ToString();
        }
    }
}
