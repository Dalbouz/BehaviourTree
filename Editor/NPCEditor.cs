using Dawud.BT.Behavior;
using UnityEditor;
using UnityEngine;

namespace Dawud.BT.Misc
{
    [CustomEditor(typeof(RobberBehavior))]
    public class RobberBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RobberBehavior npcMain = (RobberBehavior)target;

            if (GUILayout.Button("Start Behave"))
            {
                npcMain.StartBehave();
            }

            if (GUILayout.Button("Print Out Tree"))
            {
                npcMain.PrintOutTree();
            }
        }
    }
}
