using Shotgun.Core.Domain;

namespace Shotgun.Core.Rules;

public class RulesEngine
{
    private readonly TieMode _tieMode;
    private readonly Random _rng;

    public RulesEngine(TieMode tieMode, Random? rng = null)
    {
        _tieMode = tieMode;
        _rng = rng ?? new Random();
    }

    public RoundResults ResolveRound(Actions playerChoice, Actions computerChoice, int playerAmmo, int computerAmmo)
    {
        // Shotgun 
        if (playerChoice == Actions.Shotgun && computerChoice != Actions.Shotgun)
            return new RoundResults(playerChoice, computerChoice, playerAmmo - 3, computerAmmo, "Player", "You won with Shotgun!");
        if (computerChoice == Actions.Shotgun && playerChoice != Actions.Shotgun)
            return new RoundResults(playerChoice, computerChoice, playerAmmo, computerAmmo - 3, "Computer", "Computer won with Shotgun!");

        if (playerChoice == Actions.Shotgun && computerChoice == Actions.Shotgun)
        {
            var pAmmo = playerAmmo - 3;
            var cAmmo = computerAmmo - 3;

            if (_tieMode == TieMode.RandomWinner)
            {
                var winner = _rng.Next(0, 2) == 0 ? "Player" : "Computer";
                var description = winner == "Player" ? "Both used Shotgun! You win the tie!" : "Both used Shotgun! Computer wins the tie!";
                return new RoundResults(playerChoice, computerChoice, pAmmo, cAmmo, winner, description);
            }

            return new RoundResults(playerChoice, computerChoice, pAmmo, cAmmo, "None", "Both used Shotgun! It's a tie!");
        }
        // Load vs Load
        if (playerChoice == Actions.Load && computerChoice == Actions.Load)
            return new RoundResults(playerChoice, computerChoice, playerAmmo + 1, computerAmmo + 1, "None", "Both players loaded.");

        // Load vs Block
        if (playerChoice == Actions.Load && computerChoice == Actions.Block)
            return new RoundResults(playerChoice, computerChoice, playerAmmo + 1, computerAmmo, "None", "You loaded while the computer blocked.");
        if (playerChoice == Actions.Block && computerChoice == Actions.Load)
            return new RoundResults(playerChoice, computerChoice, playerAmmo, computerAmmo + 1, "None", "Computer loaded while you blocked.");

        // Block vs Block 
        if (playerChoice == Actions.Block && computerChoice == Actions.Block)
            return new RoundResults(playerChoice, computerChoice, playerAmmo, computerAmmo, "None", "Both players blocked.");

        // Shoot vs Block 
        if (playerChoice == Actions.Shoot && computerChoice == Actions.Block)
            return new RoundResults(playerChoice, computerChoice, playerAmmo - 1, computerAmmo, "None", "You shot while the computer blocked.");

        if (playerChoice == Actions.Block && computerChoice == Actions.Shoot)
            return new RoundResults(playerChoice, computerChoice, playerAmmo, computerAmmo - 1, "None", "Computer shot while you blocked.");

        // Shoot vs Shoot
        if (playerChoice == Actions.Shoot && computerChoice == Actions.Shoot)
            return new RoundResults(playerChoice, computerChoice, playerAmmo - 1, computerAmmo - 1, "None", "Both players shot each other!");

        // Shoot vs Load
        if (playerChoice == Actions.Shoot && computerChoice == Actions.Load)
            return new RoundResults(playerChoice, computerChoice, playerAmmo - 1, computerAmmo, "Player", "You shot the computer and won the game!");
        if (playerChoice == Actions.Load && computerChoice == Actions.Shoot)
            return new RoundResults(playerChoice, computerChoice, playerAmmo, computerAmmo - 1, "Computer", "Computer shot you, you lost the game! :(");

        // fallback - should never happen
        return new RoundResults(playerChoice, computerChoice, playerAmmo, computerAmmo, "None", "Unknown winner!");
    }
}