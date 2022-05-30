using BackToZeroLib;
using BackToZeroLib.GridProj;
using BackToZeroLib.SampProj;
using BackToZeroLib.SampProj.Logic;
using System;

namespace BackToZero
{
    class Program
    {
        static void Main(string[] args)
        {
            var gm = new GameLogix();

            PlayerModel player1 = gm.CreatePlayer("Player 1");
            PlayerModel player2 = gm.CreatePlayer("Player 1");
            PlayerModel winner = null;
            do
            {
                gm.DisplayShotGrid(player1);
                gm.RecordPlayerShot(player1, player2);

                // check If the active player has remaining ship location to guess
                bool isStillContinue = gm.GameStillContinue(player2);
                if (isStillContinue == true)
                {
                    (player1, player2) = (player2, player1);
                }
                else
                {
                    winner = player1;
                }
            } while (winner == null);

            CongratsWinner(winner);

            Console.ReadLine();
        }

        private static void CongratsWinner(PlayerModel winner)
        {
            Console.WriteLine();
            Console.WriteLine($"Congrats to {winner.UserName} for winning the game");
            Console.WriteLine($"{winner.UserName} tooks {GetShotCount(winner)} shots");
        }

        private static int GetShotCount(PlayerModel winner)
        {
            int shot = 0;
            foreach (var item in winner.ShotGrid)
            {
                if (item.Status != GridSpotStatux.EMPTY)
                    shot++;
            }
            return shot;
        }
    }
}
