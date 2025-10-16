using Shotgun.Core.Domain;

namespace Shotgun.Core.AI;

public class SmartAiStrategy : IAiStrategy
{
    private readonly Random _rng;
    public SmartAiStrategy(Random? rng = null) => _rng = rng ?? new Random();

    public Actions PickAction(IReadOnlyList<Actions> allowedActions, int aiAmmo, int playerAmmo)
    {
        if (allowedActions == null || allowedActions.Count == 0)
            throw new InvalidOperationException("AI is missing allowed choices.");

        bool Has(Actions a) => allowedActions.Contains(a);

        // Shotgun if possible
        if (aiAmmo >= 3 && Has(Actions.Shotgun))
            return Actions.Shotgun;

        // 1) AI has 0 ammo -> Load if possible, else Block
        if (aiAmmo == 0)
        {
            if (Has(Actions.Load)) return Actions.Load;
            if (Has(Actions.Block)) return Actions.Block;
            return allowedActions[0]; // fallback
        }

        // 2) AI has ammo (>0) and player has 0 -> Load or Shoot
        if (playerAmmo == 0)
        {
            if (Has(Actions.Shoot) && Has(Actions.Load))
                return _rng.Next(2) == 0 ? Actions.Shoot : Actions.Load;

            if (Has(Actions.Shoot)) return Actions.Shoot;
            if (Has(Actions.Load)) return Actions.Load;
            if (Has(Actions.Block)) return Actions.Block;
            return allowedActions[0];
        }

        // 3) player has ammo (>0) -> Shoot, Block, Load
        // Shoot 40%, Block 35%, Load 25%
        int roll = _rng.Next(100);
        if (roll < 40 && Has(Actions.Shoot)) return Actions.Shoot;
        if (roll < 75 && Has(Actions.Block)) return Actions.Block;
        if (Has(Actions.Load)) return Actions.Load;

        return allowedActions[0]; // fallback
    }
}
