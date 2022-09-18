using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RENTACARKUTUPHANE
{
    public class RCK
    {
        public static string logoBul(string marka)
        {
            return Directory.GetCurrentDirectory() + "\\logos\\" + marka + ".png";
        }
    }
}
