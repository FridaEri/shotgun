namespace Shotgun.Core.Domain;

public class Player
{
    public string Name { get; }
    public int Ammo { get; private set; }
    public Actions? LastAction { get; private set; }

    public Player(string name)
    {
        Name = name;
        Ammo = 0;
        LastAction = null;
    }

    public void SetAmmo(int newAmmo) => Ammo = Math.Max(0, newAmmo);
    public void SetLastAction(Actions action) => LastAction = action;

    public IReadOnlyList<Actions> AvailableActions()
    {
        var list = new List<Actions> { Actions.Block, Actions.Load };
        if (Ammo > 0) list.Add(Actions.Shoot);
        if (Ammo > 2) list.Add(Actions.Shotgun);
        return list;
    }
}