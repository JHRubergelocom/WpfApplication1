using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportSol
{
    class CommandLineOptions
    {
        [Option('a', "arcpath", Required = true)]
        public string arcpath { get; set; }

        [Option('w', "winpath", Required = true)]
        public string winpath { get; set; }

        [Option('i', "ixurl", Required = true)]
        public string ixurl { get; set; }

        [Option('u', "user", Required = true)]
        public string user { get; set; }

        [Option('p', "pwd", Required = true)]
        public string pwd { get; set; }
    }
}
