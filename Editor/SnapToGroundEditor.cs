using UnityEditor;
using UnityEngine;

public class SnapToGroundEditor : Editor
{
    private const string UndoGroupName = "Snap to Ground";

    [MenuItem("GameObject Tools/Snap to Ground %g")]
    private static void SnapToGround()
    {
        Undo.SetCurrentGroupName(UndoGroupName);

        foreach (var gameObject in Selection.gameObjects)
        {
            var collider = gameObject.GetComponent<Collider>();
            if (!collider)
            {
                Debug.LogWarning("GameObject " + gameObject.name + " does not have a collider.", gameObject);
                continue;
            }

            var lowestPoint = collider.bounds.min;
            if (!Physics.Raycast(lowestPoint + Vector3.up * 0.1f, Vector3.down, out var hit)) continue;
            Undo.RecordObject(gameObject.transform, UndoGroupName);

            var distanceToMoveDown = Vector3.Distance(lowestPoint, hit.point);
            gameObject.transform.position -= Vector3.up * distanceToMoveDown;
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
    }
}