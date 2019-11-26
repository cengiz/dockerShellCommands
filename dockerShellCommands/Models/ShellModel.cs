using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dockerShellCommands.Models
{
    public class ShellModel
    {
        public string cmd { get; set; }
        public int UseShellExecute { get; set; }
        public string WorkingDirectory { get; set; }
    }
}
