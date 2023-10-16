using Flax.Build;

public class HideNSeekEditorTarget : GameProjectEditorTarget
{
    /// <inheritdoc />
    public override void Init()
    {
        base.Init();

        // Reference the modules for editor
        Modules.Add("HideNSeek");
        Modules.Add("HideNSeekEditor");
    }
}
