using System;

namespace BattleShipLibrary
{
    public static class DisplayGameInfo
    {
        public static PlayerInfo CreatePlayer(string playerName)
        {
            PlayerInfo output = new PlayerInfo();

            Console.WriteLine($"  Player information for {playerName}");

            //User name validation
            do
            {
                output.Name = PlayerSettings.GetAndVerifyName(playerName);
                PlayerSettings.GetErrorName(output.Error);

            }
            while (output.Error == ErrorType.ErrorName);

            // Load up the shot grid
            GameLogic.InitializeGrid(output);

            // Ask the user for their 5 ship placements

            PlaceShips(output);

            // Clear
            Console.Clear();

            return output;
        }

        private static void PlaceShips(PlayerInfo model)
        {
            do
            {
                Console.Write($"  Where do you want to place ship number {model.ShipLocations.Count + 1}: ");
                string location = Console.ReadLine().Trim();

                bool isValidLocation = false;

                try
                {
                    isValidLocation = GameLogic.PlaceShip(model, location);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("  Error: " + ex.Message);
                }

                if (isValidLocation == false)
                {
                    Console.WriteLine("  That was not a valid location.  Please try again.");
                }
            } while (model.ShipLocations.Count < 5);
        }
    }
}
