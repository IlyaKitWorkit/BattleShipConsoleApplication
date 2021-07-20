using System;
using BattleShipLibrary;

namespace BattleShip
{
    class Program
    {
        static void Main(string[] args)
        {
            GameInscriptions.ShowTitleMessage();
            PlayerSettings.WelcomeMessage();

            PlayerInfo activePlayer = DisplayGameInfo.CreatePlayer("Player 1");
            PlayerInfo opponent = DisplayGameInfo.CreatePlayer("Player 2");
            PlayerInfo winner = null;

            do
            {
                DisplayShotGrid(activePlayer);

                RecordPlayerShot(activePlayer, opponent);

                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                if (doesGameContinue == true)
                {
                    // Use Tuple swap positions
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }

            } while (winner == null);

            IdentifyWinner(winner);

            Console.ForegroundColor = ConsoleColor.Blue;

            DisplayShotGrid(winner);

            GameInscriptions.ShowGameOverMessage();

            Console.ReadKey();
        }

        private static void DisplayShotGrid(PlayerInfo activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].PointLetter;

            foreach (var gridPoint in activePlayer.ShotGrid)
            {
                if (gridPoint.PointLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridPoint.PointLetter;
                }

                if (gridPoint.Status == GridPointState.Empty)
                {
                    Console.Write($" {gridPoint.PointLetter}{gridPoint.PointNumber} ");

                }
                else if (gridPoint.Status == GridPointState.Hit)
                {
                    Console.Write(" X  ");
                }
                else if (gridPoint.Status == GridPointState.Miss)
                {
                    Console.Write(" O  ");
                }
                else
                {
                    Console.Write(" ?  ");
                }
            }
            Console.WriteLine();
        }

        private static void RecordPlayerShot(PlayerInfo activePlayer, PlayerInfo opponent)
        {
            string row = "";
            int column = 0;

            bool isValidShot;
            do
            {
                string shot = AskForShot(activePlayer);
                try
                {
                    (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                    isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error: " + ex.Message);
                    isValidShot = false;
                }

                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid Shot Location.  Please try again.");
                }

            } while (isValidShot == false);

            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);

            DisplayShotResults(row, column, isAHit);
        }

        private static string AskForShot(PlayerInfo player)
        {
            Console.Write($"{player.Name}, please enter your shot selection: ");
            string output = Console.ReadLine().Trim();

            return output;
        }

        private static void DisplayShotResults(string row, int column, bool isAHit)
        {
            if (isAHit)
            {
                Console.WriteLine($"{row}{column} is a Hit!");
            }
            else
            {
                Console.WriteLine($"{row}{column} is a miss.");
            }

            Console.WriteLine();
        }

        private static void IdentifyWinner(PlayerInfo winner)
        {
            Console.WriteLine($"Congratulations to {winner.Name} for winning!");
            Console.WriteLine($"{winner.Name} took {GameLogic.GetShotCount(winner)} shots.");
            Console.WriteLine();
        }
    }
}
