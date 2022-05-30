using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackToZeroLib.SampProj.Logic
{
    public class GameLogix
    {
        public string AskUserName()
        {
            Console.WriteLine();
            Console.WriteLine();

            Console.Write("What is your name: ");
            string str = Console.ReadLine();
            return str;
        }
        public PlayerModel CreatePlayer(string player)
        {
            var output = new PlayerModel();

            Console.WriteLine($"Player information for player {player} ");

            output.UserName = AskUserName();

            InitializedGrid(output);

            PlayerPlacingTheirShip(output);

            Console.Clear();

            return output;
        }

        public bool GameStillContinue(PlayerModel player2)
        {
            bool isvalid = false;

            foreach (var item in player2.ShipLocation)
            {
                if (item.Status != GridSpotStatux.SUNK)
                    isvalid = true;
            }

            return isvalid;
        }

        public void RecordPlayerShot(PlayerModel player1, PlayerModel player2)
        {
            bool isValidShot = false;

            do
            {
                string row = "";
                int column = 0;
                Console.Write($"{player1.UserName}, Guess the opponent ship location to gain points ");
                string location = Console.ReadLine();
                try
                {
                    (row, column) = SplitingRowAndColumn(location);
                    isValidShot = ValidatePlayerShot(player1, row,column);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if(isValidShot == false)
                {
                    Console.WriteLine("Player shot is Invalid");
                }
                bool isHit = IdentifyIfHitOnOpponent(player2, row, column);

                // Active player that will show if the ship location guess
                // is mark or miss on that player shot grid view.
                MarkAsHit(isHit, row, column, player1);

                DisplayIfHit(isHit, row, column);
            } while (isValidShot == false);
        }

        private void MarkAsHit(bool isHit, string row, int column, PlayerModel player1)
        {
            foreach (var item in player1.ShotGrid)
            {
                if(item.SpotLetter == row.ToUpper() && item.SpotNumber == column)
                {
                    if (isHit)
                    {
                        item.Status = GridSpotStatux.HIT;
                    }
                    else
                    {
                        item.Status = GridSpotStatux.MISS;

                    }
                }
            }
        }

        private void DisplayIfHit(bool isHit, string row, int column)
        {
            if (isHit)
            {
                Console.WriteLine("=====================");
                Console.WriteLine($"{row.ToUpper()}{column} is HIT");
                Console.WriteLine("=====================");

            }
            else
            {
                Console.WriteLine("=====================");


                Console.WriteLine($"{row.ToUpper()}{column} is MISS");
                Console.WriteLine("=====================");


            }
        }

        private bool IdentifyIfHitOnOpponent(PlayerModel player2, string row, int column)
        {
            bool isvalid = false;
            foreach (var item in player2.ShipLocation)
            {
                if (item.SpotLetter == row.ToUpper() && item.SpotNumber == column)
                {
                    isvalid = true;
                    item.Status = GridSpotStatux.SUNK;
                }
            }
            
            return isvalid;
        }

        private bool ValidatePlayerShot(PlayerModel player1, string res, int column)
        {
            bool isvalid = false;

            foreach (var item in player1.ShotGrid)
            {
                if (item.SpotLetter == res.ToUpper() && item.SpotNumber == column)
                    isvalid = true;
            }

            return isvalid;
        }

        public void DisplayShotGrid(PlayerModel player1)
        {
            var currentRow = player1.ShotGrid[0].SpotLetter;
            foreach (var item in player1.ShotGrid)
            {
                currentRow = DisplayShot(currentRow, item);

            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private static string DisplayShot(string currentRow, GridSpotModel item)
        {
            if (item.SpotLetter != currentRow)
            {
                Console.WriteLine();
                currentRow = item.SpotLetter;
            }
            if (item.Status == GridSpotStatux.EMPTY)
            {
                Console.Write($" {item.SpotLetter}{item.SpotNumber} ");
            }
            else if (item.Status == GridSpotStatux.HIT)
            {
                Console.Write(" X  ");
            }
            else if (item.Status == GridSpotStatux.MISS)
            {
                Console.Write(" O  ");
            }
            else
            {
                Console.Write(" ?  ");

            }

            return currentRow;
        }

        private void PlayerPlacingTheirShip(PlayerModel output)
        {
            do
            {
                string location = PlacingShipPrompt(output);
                bool isValidShip = false;
                try
                {
                    isValidShip = PlaceShip(location, output);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (isValidShip == false)
                {
                    Console.WriteLine("The ship you`re creating is not accurate.");
                }
            } while (output.ShipLocation.Count() < 2);
        }

        private static string PlacingShipPrompt(PlayerModel output)
        {
            Console.WriteLine();
            Console.Write($"Where do you want to place you ship" +
                $" number {output.ShipLocation.Count() + 1}: ");
            string location = Console.ReadLine();
            return location;
        }

        private bool PlaceShip(string location, PlayerModel output)
        {
            bool isValid = false;
            (string row, int column) = SplitingRowAndColumn(location);

            bool isSpotOpen, validGrid;
            ValidAngSpotOpen(output, row, column, out isSpotOpen, out validGrid);

            if (isSpotOpen && validGrid)
            {
                AddingShipLocation(output, row, column);
                isValid = true;
            }

            return isValid;
        }

        private void ValidAngSpotOpen(PlayerModel output, string row, int column, out bool isSpotOpen, out bool validGrid)
        {
            isSpotOpen = ValidateShipLocation(output, row, column);
            validGrid = ValidateShotGrid(output, row, column);
        }

        private static void AddingShipLocation(PlayerModel output, string row, int column)
        {
            output.ShipLocation.Add(new GridSpotModel
            {
                SpotLetter = row.ToUpper(),
                SpotNumber = column,
                Status = GridSpotStatux.SHIP
            });
        }

        private bool ValidateShotGrid(PlayerModel output, string row, int column)
        {
            bool isValid = false;
            foreach (var item in output.ShotGrid)
            {
                if (item.SpotLetter == row.ToUpper() && item.SpotNumber == column)
                    isValid = true;
            }
            return isValid;
        }

        private bool ValidateShipLocation(PlayerModel output, string row, int column)
        {
            bool isValid = true;
            foreach (var item in output.ShipLocation)
            {
                isValid = IsValidShipLoc(item, row, column);
            }
            return isValid;
        }
 
        private bool IsValidShipLoc(GridSpotModel item, string row, int column)
        {
            bool isValid = true;
            if (item.SpotLetter == row.ToUpper() && item.SpotNumber == column)
                isValid = false;
            return isValid;
        }
        private (string row, int column) SplitingRowAndColumn(string location)
        {
            string row = "";
            int column = 0;

            if (location.Length != 2)
                throw new ArgumentException("Ship Location is Invalid", location);

            char[] locArray = location.ToArray();

            row = locArray[0].ToString();
            column = int.Parse(locArray[1].ToString());

            return (row, column);
        }

        // FIRST 1!
        private void InitializedGrid(PlayerModel output)
        {
            var letter = new List<string>()
            {
                "A","B"
            };
            var number = new List<int>() { 1, 2, };
            foreach (var le in letter)
            {
                foreach (var num in number)
                {
                    output.ShotGrid.Add(new GridSpotModel
                    {
                        SpotLetter = le,
                        SpotNumber = num,
                        Status = GridSpotStatux.EMPTY
                    });
                }
            }

        }
    }
}
