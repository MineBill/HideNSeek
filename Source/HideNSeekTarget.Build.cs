using Flax.Build;

public class HideNSeekTarget : GameProjectTarget
{
    /// <inheritdoc />
    public override void Init()
    {
        base.Init();

        // Reference the modules for game
        Modules.Add("HideNSeek");
    }
}
