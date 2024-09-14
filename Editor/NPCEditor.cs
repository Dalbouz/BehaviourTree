using Dawud.BT.Behaviour;
using Dawud.BT.General;
using UnityEditor;
using UnityEngine;

namespace Dawud.BT.Misc
{
    [CustomEditor(typeof(RobberBehaviour))]
    public class NPCEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RobberBehaviour npcMain = (RobberBehaviour)target;

            if (GUILayout.Button("Start Behave"))
            {
                npcMain.StartBehave();
            }
        }
    }
}
