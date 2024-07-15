using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ASCIIMusicVisualiser8.Types
{
    internal interface IHierarchy
    {
        public string name { get;}

        // isActive propogates down to children
        public void HandleNext(string indent, bool last, double time, bool isActive);


    }
}
