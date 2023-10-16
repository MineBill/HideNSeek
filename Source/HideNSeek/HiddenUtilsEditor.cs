using System.Collections.Generic;
using FlaxEditor;
using FlaxEditor.GUI.ContextMenu;
using FlaxEditor.SceneGraph;
using FlaxEngine;

namespace HideNSeek;

/// <summary>
/// Adds the ability to lock/unlock actors by adding the actions to the context menu.
/// </summary>
public class HiddenUtilsEditor: EditorPlugin
{
    private class LockAction : IUndoAction
    {
        public string ActionString { get; } = "Lock";
        private List<Actor> _actors;

        public LockAction(List<Actor> actors)
        {
            _actors = actors;
        }
        
        public void Dispose()
        {
        }

        public void Do()
        {
            foreach (var actor in _actors)
            {
                actor.HideFlags = HideFlags.DontSelect;
            }
        }

        public void Undo()
        {
            foreach (var actor in _actors)
            {
                actor.HideFlags = HideFlags.None;
            }
        }
    }

    private class UnlockAction : IUndoAction
    {
        public string ActionString { get; } = "Unlock";

        private List<Actor> _actors;
        
        public UnlockAction(List<Actor> actors)
        {
            _actors = actors;
        }
        
        public void Dispose()
        {
        }

        public void Do()
        {
            foreach (var actor in _actors)
            {
                actor.HideFlags = HideFlags.None;
            }
        }

        public void Undo()
        {
            foreach (var actor in _actors)
            {
                actor.HideFlags = HideFlags.DontSelect;
            }
        }
    }
    
    /// <inheritdoc />
    public override void InitializeEditor()
    {
        base.InitializeEditor();
        Editor.Windows.SceneWin.ContextMenuShow += OnContextMenuShow;
    }

    private void OnContextMenuShow(ContextMenu menu)
    {
        menu.AddSeparator();
        if (Editor.SceneEditing.SelectionCount == 1)
        {
            var graphNode = Editor.SceneEditing.Selection[0];
            if (graphNode is not ActorNode actorNode) return;
            
            var isLocked = actorNode.Actor.HideFlags.HasFlag(HideFlags.DontSelect);
            
            var actors = new List<Actor> { actorNode.Actor };
            menu.AddButton(isLocked ? "Unlock Actor" : "Lock Actor", () =>
            {
                IUndoAction action = isLocked ? new UnlockAction(actors) : new LockAction(actors);
                Editor.Undo.AddAction(action);
                action.Do();
            });
        }
        else
        {
            var actors = Editor.SceneEditing.Selection.ConvertAll(input => (input as ActorNode)?.Actor);
            menu.AddButton("Force Locked", () =>
            {
                var action = new LockAction(actors);
                Editor.Undo.AddAction(action);
                action.Do();
            });

            menu.AddButton("Force Unlocked", () =>
            {
                var action = new UnlockAction(actors);
                Editor.Undo.AddAction(action);
                action.Do();
            });
        }
    }

    /// <inheritdoc />
    public override void DeinitializeEditor()
    {
        Cleanup();
        base.DeinitializeEditor();
    }

    private void Cleanup()
    {
        Editor.Windows.SceneWin.ContextMenuShow -= OnContextMenuShow;
    }
}


