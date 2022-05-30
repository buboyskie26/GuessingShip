using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackToZeroLib.SampProj
{
    public class PlayerModel
    {
        public string UserName { get; set; }

        public List<GridSpotModel> ShotGrid { get; set; } = new List<GridSpotModel>();
        public List<GridSpotModel> ShipLocation { get; set; } = new List<GridSpotModel>();
    }
}
