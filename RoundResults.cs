using System.Security.Cryptography.X509Certificates;

namespace Shotgun.Core.Domain;

public sealed class RoundResults
{
    public Actions PlayerChoice { get; }
    public Actions ComputerChoice { get; }
    public int NewPlayerAmmo { get; }
    public int NewComputerAmmo { get; }
    public string Winner { get; }
    public string Description { get; }


    public RoundResults(Actions playerChoice, Actions computerChoice, int newPlayerAmmo, int newComputerAmmo, string winner, string description)
    {
        PlayerChoice = playerChoice;
        ComputerChoice = computerChoice;
        NewPlayerAmmo = Math.Max(0, newPlayerAmmo);
        NewComputerAmmo = Math.Max(0, newComputerAmmo);
        Winner = winner;
        Description = description;
    }

    public bool HasWinner() => Winner != "None";
}