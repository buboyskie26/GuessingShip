using BackToZeroLib.GridProj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackToZeroLib
{
    public class GameLogic : IGameLogic
    {
        public string AskPlayersName()
        {
            Console.WriteLine("Player Information");
            Console.WriteLine("===================");
            Console.WriteLine("Whats is your name: ");

            string playerInfo = Console.ReadLine();
            return playerInfo;
        }

        public void RecordPlayerShot(PlayerInfoModel player1, PlayerInfoModel player2)
        {
            bool isValidShot = false;
            string row = "";
            int column = 0;
            do
            {
                try
                {
                    Console.Write($"{player1.UserName}, Plese guess the opponent ship: ");
                    string shot = Console.ReadLine();

                    (row, column) = SplittingFirstAndSecond(shot);

                    isValidShot = ValidatingShot(player1, row, column);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    isValidShot = false;
                }
                if(isValidShot == false)
                {
                    Console.WriteLine("Try another attack.");
                }
            } while (isValidShot = false);

            bool isHit = IdentifyShotResult(player2, row, column);

            MarkShotResult(player1, row, column, isHit);

            DisplayShotResult(row, column, isHit);

        }

        private void MarkShotResult(PlayerInfoModel player1, string row, int column, bool isHit)
        {
            foreach (var item in player1.ShotGrid)
            {
                if(item.SpotLetter == row.ToLower() && item.SpotNumber == column)
                {
                    if(isHit)
                    {
                        item.GridStatus = GridSpotStatus.Hit;

                    }
                    else
                    {
                        item.GridStatus = GridSpotStatus.Miss;

                    }
                }
            }
        }

        public bool GameStillAlive(PlayerInfoModel player2)
        {
            bool isAlive = false;
            foreach (var item in player2.ShipLocations)
            {
                if (item.GridStatus != GridSpotStatus.Sank)
                {
                    isAlive = true;
                }
            }
            return isAlive;
        }

        private void DisplayShotResult(string row, int column, bool isHit)
        {
            if (isHit)
            {
                Console.WriteLine($"{row} {column} is hit");
            }
            else
            {
                Console.WriteLine($"{row} {column} is a missed");

            }
            Console.WriteLine();
        }

        private bool IdentifyShotResult(PlayerInfoModel player2, string row, int column)
        {
            // Check if player 1 has a spotletter and number on player2
            // if true mark his gridshiplocation as sunk

            // row and column parameters belongs to player1
            bool hit = false;
            foreach (var item in player2.ShipLocations)
            {
                if (item.SpotLetter == row.ToLower() && item.SpotNumber == column)
                {
                    hit = true;
                    item.GridStatus = GridSpotStatus.Sank;
                }
            }
            return hit;
        }

        private bool ValidatingShot(PlayerInfoModel player1, string row, int column)
        {
            bool isValid = false;
            foreach (var item in player1.ShotGrid)
            {
                if(item.SpotLetter == row.ToLower() && item.SpotNumber == column)
                {
                    if(item.GridStatus == GridSpotStatus.Empty)
                        isValid = true; 
                    
                }
            }
            return isValid;
        }

        public void DisplayPlayerShot(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if (gridSpot.GridStatus == GridSpotStatus.Empty)
                {
                    Console.Write($" { gridSpot.SpotLetter }{ gridSpot.SpotNumber } ");
                }
                else if (gridSpot.GridStatus == GridSpotStatus.Hit)
                {
                    Console.Write(" X  ");
                }
                else if (gridSpot.GridStatus == GridSpotStatus.Miss)
                {
                    Console.Write(" O  ");
                }
                else
                {
                    Console.Write(" ?  ");
                }
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        public PlayerInfoModel CreatePlayer(string playerName)
        {
            var info = new PlayerInfoModel();

            Console.WriteLine($"Player information for: {playerName}");

            // Ask player User
            info.UserName = AskPlayersName();

            // Initialized the grid.
            InitializedGrid(info);

            // Players placing ship
            PlacingShip(info);

            // Clear
            Console.Clear();

            return info;
        }

        private void PlacingShip(PlayerInfoModel info)
        {
           
            do
            {
                Console.Write($"Where do you want to place your ship" +
                  $" number {info.ShipLocations.Count() + 1}: ");

                string location = Console.ReadLine();
                bool isValid = false;

                try
                {
                    isValid = PlaceShipLocation(location, info);

                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);

                }
                if(isValid == false)
                {
                    Console.WriteLine("Ship location was false.");
                    Console.WriteLine();
                }
            } while (info.ShipLocations.Count() < 2 );

        }

        private bool PlaceShipLocation(string location, PlayerInfoModel info)
        {
            bool isValid = false;
            (string row, int column) = SplittingFirstAndSecond(location);

            bool isValidGrid = ValidateShotGrid(info, row, column);
            bool isSpotOpen = ValidateShipLocation(info, row, column);

            if(isValidGrid && isSpotOpen)
            {
                info.ShipLocations.Add(new GridSpotModel()
                {
                    SpotLetter = row.ToLower(),
                    SpotNumber = column,
                    GridStatus = GridSpotStatus.Ship
                });
                isValid = true;
            }
               
            return isValid;
        }

        private bool ValidateShipLocation(PlayerInfoModel info, string row, int column)
        {
            bool isValidSpot = true;

            foreach (var item in info.ShipLocations)
            {
                if (item.SpotLetter == row.ToLower() && item.SpotNumber == column)
                    isValidSpot = false;
            }
            return isValidSpot;
        }

        private bool ValidateShotGrid(PlayerInfoModel info, string row, int column)
        {
            bool isValidShotGrid = false;

            foreach (var item in info.ShotGrid)
            {
                if (item.SpotLetter == row.ToLower() && item.SpotNumber == column)
                {
                    isValidShotGrid = true;

                }
            }

            return isValidShotGrid;
        }

        private (string row, int column) SplittingFirstAndSecond(string location)
        {
            
            string row = "";
            int column = 0;

            // check if location length if it is not 2
            if(location.Length != 2)
            {
                throw new ArgumentException($"Invalid ship location", location);
            }
            char[] shipLocation = location.ToArray();

            // Get the first as string, second as int 

            row = shipLocation[0].ToString();
            column = int.Parse(shipLocation[1].ToString());

            return (row, column);
        }
        public void InitializedGrid(PlayerInfoModel output)
        {
            List<string> letter = new List<string>()
            {
                "a","b"
            };
            List<int> number = new List<int>()
            {
                1,2
            };
            foreach (var let in letter)
            {
                foreach (var num in number)
                {
                 /*   AddGridSpot(let, num,output);*/
                    output.ShotGrid.Add(new GridSpotModel()
                    {
                        SpotLetter = let,
                        SpotNumber = num,
                        GridStatus = GridSpotStatus.Empty
                    });
                }
            }
        }
      
    }
}
