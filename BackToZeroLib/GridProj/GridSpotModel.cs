
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackToZeroLib.GridProj
{
    public class GridSpotModel
    {
        public string SpotLetter { get; set; }
        public int SpotNumber { get; set; }
        public GridSpotStatus GridStatus { get; set; } = GridSpotStatus.Empty;
    }
}
