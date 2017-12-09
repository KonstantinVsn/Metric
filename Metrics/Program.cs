using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] fileArray = Directory.GetFiles(@"E:\Konstantiv\DefCoins", "*.cs", SearchOption.AllDirectories);

            var analyzer = new Analyzer(fileArray);
            Console.ReadKey();
        }
    }
}
