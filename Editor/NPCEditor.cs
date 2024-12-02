using KrampStudio.BT.Behavior;
using KrampStudio.BT.General;
using UnityEditor;
using UnityEngine;

namespace KrampStudio.BT.Misc
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

    [CustomEditor(typeof(CopBehavior))]
    public class CopBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CopBehavior npcMain = (CopBehavior)target;

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
