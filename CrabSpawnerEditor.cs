using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CrabSpawner))]
public class CrabSpawnerEditor : Editor
{
    private void OnSceneGUI()
    {
        CrabSpawner spawner = (CrabSpawner)target;

        if (Event.current.shift && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                spawner.waypoints.Add(hit.point);
                EditorUtility.SetDirty(spawner); 
            }

            Event.current.Use();
        }

        for (int i = 0; i < spawner.waypoints.Count; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newWaypointPosition = Handles.PositionHandle(spawner.waypoints[i], Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spawner, "Move Waypoint");
                spawner.waypoints[i] = newWaypointPosition;
                EditorUtility.SetDirty(spawner);
            }
        }
    }
}
