


using System.Collections.Generic;

namespace BackToZeroLib.SampProj
{
    public class GridSpotModel
    {
        public string SpotLetter { get; set; }
        public int SpotNumber { get; set; }
        public GridSpotStatux Status { get; set; } = GridSpotStatux.EMPTY;
    }
}
