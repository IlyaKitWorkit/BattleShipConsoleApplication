using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShipLibrary
{
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerInfo model)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach (string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddGridPoint(model, letter, number);
                }
            }
        }

        private static void AddGridPoint(PlayerInfo model, string letter, int number)
        {
            GridPoint spot = new GridPoint
            {
                PointLetter = letter,
                PointNumber = number,
                Status = GridPointState.Empty
            };

            model.ShotGrid.Add(spot);
        }

        public static bool PlaceShip(PlayerInfo model, string location)
        {
            bool output = false;
            (string row, int column) = SplitShotIntoRowAndColumn(location);

            bool isValidLocation = ValidateGridLocation(model, row, column);
            bool isPointOpen = ValidateShipLocation(model, row, column);

            if (isValidLocation && isPointOpen)
            {
                model.ShipLocations.Add(new GridPoint
                {
                    PointLetter = row.ToUpper(),
                    PointNumber = column,
                    Status = GridPointState.Ship
                });

                output = true;
            }

            return output;
        }

        private static bool ValidateShipLocation(PlayerInfo model, string row, int column)
        {
            bool isValidLocation = true;

            foreach (var ship in model.ShipLocations)
            {
                if (ship.PointLetter == row.ToUpper() && ship.PointNumber == column)
                {
                    isValidLocation = false;
                }
            }

            return isValidLocation;
        }

        private static bool ValidateGridLocation(PlayerInfo model, string row, int column)
        {

            bool isValidLocation = false;

            foreach (var ship in model.ShotGrid)
            {
                if (ship.PointLetter == row.ToUpper() && ship.PointNumber == column)
                {
                    isValidLocation = true;
                }
            }

            return isValidLocation;
        }

        public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
        {
            if (shot.Length != 2)
            {
                throw new ArgumentException("  This was an invalid shot type.", "shot");
            }

            char[] shotArray = shot.ToArray();

            string row = shotArray[0].ToString();
            int column = int.Parse(shotArray[1].ToString());
            return (row, column);
        }

        public static bool PlayerStillActive(PlayerInfo player)
        {
            bool isActive = false;

            foreach (var ship in player.ShipLocations)
            {
                if (ship.Status != GridPointState.Sunk)
                {
                    isActive = true;
                }
            }

            return isActive;
        }

        public static bool ValidateShot(PlayerInfo player, string row, int column)
        {
            bool isValidshot = false;

            foreach (var gridSpot in player.ShotGrid)
            {
                if (gridSpot.PointLetter == row.ToUpper() && gridSpot.PointNumber == column)
                {
                    if (gridSpot.Status == GridPointState.Empty)
                    {
                        isValidshot = true;
                    }
                }
            }

            return isValidshot;
        }

        public static bool IdentifyShotResult(PlayerInfo opponent, string row, int column)
        {
            bool isAHit = false;

            foreach (var ship in opponent.ShipLocations)
            {
                if (ship.PointLetter == row.ToUpper() && ship.PointNumber == column)
                {
                    isAHit = true;
                    ship.Status = GridPointState.Sunk;
                }
            }

            return isAHit;
        }

        public static void MarkShotResult(PlayerInfo player, string row, int column, bool isAHit)
        {
            foreach (var gridSpot in player.ShotGrid)
            {
                if (gridSpot.PointLetter == row.ToUpper() && gridSpot.PointNumber == column)
                {
                    if (isAHit)
                    {
                        gridSpot.Status = GridPointState.Hit;
                    }
                    else
                    {
                        gridSpot.Status = GridPointState.Miss;
                    }
                }
            }
        }

        public static int GetShotCount(PlayerInfo player)
        {
            int shotCount = 0;

            foreach (var shot in player.ShotGrid)
            {
                if (shot.Status != GridPointState.Empty)
                {
                    shotCount += 1;
                }
            }

            return shotCount;
        }
    }
}
