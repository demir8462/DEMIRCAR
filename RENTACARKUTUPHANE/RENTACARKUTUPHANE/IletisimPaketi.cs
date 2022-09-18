using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RENTACARKUTUPHANE
{
    [System.Serializable]
    public class IletisimPaketi
    {
        public enum PAKETTIPI{LOGIN,SIGNUP,CEVAP,LVI,KIRAISTEK}
        public User user;
        public PAKETTIPI tip;
        public bool cevap;
        public string detay;
        public List<string[]> LVI;
        public string tc,telno,teslim,geriteslim,ID,tahsilat;
        
    }
}
