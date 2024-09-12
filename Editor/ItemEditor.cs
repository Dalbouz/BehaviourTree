using UnityEditor;
using UnityEngine;

namespace Dawud.BT.Misc
{
    /// <summary>
    /// 
    /// </summary>
    public static class ItemIDGenerator
    {
        private static int currentID = 0; // Start from 0 or load from a file

        // This function is called whenever a new item is created
        public static void AssignID(UniqueItemData item)
        {
            item.ID = ++currentID;
            EditorUtility.SetDirty(item); // Mark the object as dirty to ensure changes are saved
        }

        // Optionally, save/load ID from disk (to maintain the currentID after restarting Unity)
        public static void LoadCurrentID()
        {
            currentID = EditorPrefs.GetInt("ItemIDGenerator_CurrentID", 0); // Load the ID from EditorPrefs
        }

        public static void SaveCurrentID()
        {
            EditorPrefs.SetInt("ItemIDGenerator_CurrentID", currentID); // Save the ID to EditorPrefs
        }

        public static void DecrementCurrentID()
        {
            currentID--;
            if (currentID < 0)
            {
                Debug.Log("ID is 0");
                currentID = 0;
            }
        }

        public static int GetNextIDNumber()
        {
            return currentID + 1;
        }
    }

    [CustomEditor(typeof(UniqueItemData))]
    public class ItemEditor : Editor
    {
        private void OnEnable()
        {
            ItemIDGenerator.LoadCurrentID(); // Load the current ID when the editor starts
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UniqueItemData item = (UniqueItemData)target;

            if (GUILayout.Button("Assign Next Unique ID: " + ItemIDGenerator.GetNextIDNumber()))
            {
                ItemIDGenerator.AssignID(item);
                ItemIDGenerator.SaveCurrentID();
            }

            if (GUILayout.Button("Decrement ID"))
            {
                ItemIDGenerator.DecrementCurrentID();
                ItemIDGenerator.SaveCurrentID();
            }
        }
    }
}