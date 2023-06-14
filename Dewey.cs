using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrainer
{


    // Troelsen, A. and Japikse, P. 2017. Pro c#7 with .NET and .NET Core. 8th ed. New York: Apress.

    // CLASS OBJECTS TO STORE ELEMENT INFORMATION  Troelsen, and Japikse ( 2017)
    public class Dewey
    {
        public string callDesc { get; set; }

        public string callNum { get; set; }

        public Dewey(string callNum, string callDesc)
        {
            this.callDesc = callDesc;
            this.callNum = callNum;
        }

        public override string  ToString()
        {
            string re = $"{this.callNum} {this.callDesc}";
            return re;
        }

        protected bool Equals(Dewey obj)
        {
            return callDesc == obj.callDesc;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals (this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Dewey)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
