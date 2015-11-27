using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleMatcher
{
    class Program
    {

        static void Main(string[] args)
        {
            MatchSubs m = new MatchSubs();
            string folderPath = args[0]; //Get showPath from first CMD Line Argument
            m.renameSubfile(folderPath);
           
        }
    }
}
