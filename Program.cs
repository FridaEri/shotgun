using Shotgun.Core.AI;
using Shotgun.Core.Domain;
using Shotgun.Core.Services;

namespace Shotgun.ConsoleApp;

class Program
{
    static void Main()
    {


        var controller = new GameController(TieMode.ShootVsShoot, new SmartAiStrategy());
        Console.WriteLine("Welcome to Shotgun!");
        Console.WriteLine("You and the AI start with 0 bullets.");
        Console.WriteLine("Actions: Load (1), Block (2), Shoot (3), Shotgun (4 - requires 3 bullets)");
        Console.WriteLine("If both players shoot, it's a tie and both lose 1 bullet.");
        Console.WriteLine("If both players use Shotgun, the winner is decided by the game mode.");
        Console.WriteLine("You cant shoot or use Shotgun without enough bullets.");
        Console.WriteLine("First to use Shotgun or eliminate the opponent wins!");

        while (controller.Winner == null)
        {
            // Show status
            Console.WriteLine($"\nAmmo - You: {controller.Player.Ammo} | Computer: {controller.Computer.Ammo}");

            // Show allowed actions
            var allowed = controller.GetPlayerAllowedActions();
            for (int i = 0; i < allowed.Count; i++)
                Console.WriteLine($"{i + 1}. {allowed[i]}");

            Console.Write("Choose your action: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > allowed.Count)
            {
                Console.WriteLine("Invalid choice. Please try again.");
                continue;
            }
            var playerChoice = allowed[choice - 1];
            var result = controller.PlayRound(playerChoice);
            Console.WriteLine($"\nYou chose: {result.PlayerChoice} | Computer chose: {result.ComputerChoice}");
            Console.WriteLine(result.Description);

            if (result.HasWinner())
                Console.WriteLine($"\nGame Over! {result.Winner} wins!");
        }

        Console.WriteLine("Thanks for playing!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}