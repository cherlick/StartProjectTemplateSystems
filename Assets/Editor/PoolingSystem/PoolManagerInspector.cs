using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProjectNameTemplate.PoolingSystem
{
    [CustomEditor(typeof(PoolingManager))]
    public class PoolManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PoolingManager poolManager = (PoolingManager) target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("In Editor Mode");
            if (GUILayout.Button("Create pools"))
            {
                poolManager.CreatePoolsFromEditor();
            }
            if (GUILayout.Button("Delete all objets from pools"))
            {
                poolManager.DeletePoolsFromEditor();
            }
        }
    }
}