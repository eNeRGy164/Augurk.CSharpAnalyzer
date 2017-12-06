using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cucumis
{
    /// <summary>
    /// We all know of those...
    /// </summary>
    public class StubbornGherkin : PickyGherkin
    {
        internal override void CutVine()
        {
            base.CutVine();
            // I don't do WriteLine!
            Console.Write("StubbornGherkin: Fine, cut the vine! If that's how you want it...");
        }
    }
}
