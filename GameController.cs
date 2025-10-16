using Shotgun.Core.AI;
using Shotgun.Core.Domain;
using Shotgun.Core.Rules;

namespace Shotgun.Core.Services;

public class GameController
{
    public Player Player { get; }
    public Player Computer { get; }
    private readonly IAiStrategy _ai;
    private readonly RulesEngine _rules;

    public string? Winner { get; private set; }

    public GameController(TieMode tieMode, IAiStrategy ai)
    {
        Player = new Player("You");
        Computer = new Player("Computer");
        _ai = ai;
        _rules = new RulesEngine(tieMode);
        Winner = null;
    }

    public IReadOnlyList<Actions> GetPlayerAllowedActions() => Player.AvailableActions();
    public RoundResults PlayRound(Actions playerChoice)
    {
        if (!GetPlayerAllowedActions().Contains(playerChoice))
            throw new InvalidOperationException("Player made an invalid choice.");

        var aiAllowed = Computer.AvailableActions();
        var aiChoice = _ai.PickAction(aiAllowed, Computer.Ammo, Player.Ammo);
        var result = _rules.ResolveRound(playerChoice, aiChoice, Player.Ammo, Computer.Ammo);

        Player.SetLastAction(playerChoice);
        Player.SetAmmo(result.NewPlayerAmmo);
        Computer.SetLastAction(aiChoice);
        Computer.SetAmmo(result.NewComputerAmmo);

        if (result.Winner != "None")
            Winner = result.Winner;
        return result;
    }

    public void StartNewGame()
    {
        Player.SetAmmo(0);
        Computer.SetAmmo(0);
        Winner = null;
    }
}