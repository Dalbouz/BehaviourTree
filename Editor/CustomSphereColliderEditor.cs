namespace Dawud.BT.Misc
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(SphereCollider))]
    public class CustomSphereColliderEditor : Editor
    {
        void OnSceneGUI()
        {
            // Get the selected sphere collider
            SphereCollider sphereCollider = (SphereCollider)target;

            // Set the color for the handle
            Handles.color = Color.cyan;

            // Draw a wireframe sphere at the collider's position and radius
            Handles.DrawWireDisc(sphereCollider.transform.position + sphereCollider.center, Vector3.up, sphereCollider.radius);

            // Optionally: Make the radius adjustable via a draggable handle
            EditorGUI.BeginChangeCheck();
            float newRadius = Handles.RadiusHandle(Quaternion.identity, sphereCollider.transform.position + sphereCollider.center, sphereCollider.radius);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(sphereCollider, "Changed Sphere Collider Radius");
                sphereCollider.radius = newRadius;
            }
        }
    }
}