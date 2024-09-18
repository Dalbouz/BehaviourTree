using Dawud.BT.Behaviour;
using UnityEditor;
using UnityEngine;

namespace Dawud.BT.Misc
{
    [CustomEditor(typeof(RobberBehaviour))]
    public class RobberBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RobberBehaviour npcMain = (RobberBehaviour)target;

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
