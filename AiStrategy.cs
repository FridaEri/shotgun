using Shotgun.Core.Domain;

namespace Shotgun.Core.AI;

public interface IAiStrategy
{
    Actions PickAction(IReadOnlyList<Actions> allowedActions, int aiAmmo, int playerAmmo);
}
