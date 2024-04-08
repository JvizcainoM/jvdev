using UnityEditor;

public class LockInspector 
{
    [MenuItem("Edit/Lock Inspector %l")]
    public static void ToggleInspectorLock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }
    
    [MenuItem("Edit/Lock Inspector %l", true)]
    public static bool ToggleInspectorLockValidate()
    {
        Menu.SetChecked("Edit/Lock Inspector %l", ActiveEditorTracker.sharedTracker.isLocked);
        return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
    }
}
