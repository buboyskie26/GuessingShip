using BackToZeroLib.GridProj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackToZeroLib
{
    public interface IGameLogic
    {
        public PlayerInfoModel CreatePlayer(string playerName);
        public string AskPlayersName();
        public void InitializedGrid(PlayerInfoModel playerInfoModel);
    }
}
