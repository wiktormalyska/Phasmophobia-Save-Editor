using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhasmoSaveEditor
{
    public class PlayerData {
        public int value { get; set; }
    }
    public class Experience : PlayerData{ }

    public class PlayersMoney : PlayerData { }

    public class Level : PlayerData { }

    public class Prestige : PlayerData { }
}
