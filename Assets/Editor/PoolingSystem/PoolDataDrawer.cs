using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace ProjectNameTemplate.PoolingSystem
{
    [CustomPropertyDrawer(typeof(PoolData))]
    public class PoolDataDrawer: PropertyDrawer 
    {    

        private const float FOLDOUT_HEIGHT = 16f;

        private float propertyLines = 5.5f;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = property.isExpanded ? FOLDOUT_HEIGHT * propertyLines : FOLDOUT_HEIGHT;

            return height;
        }

         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float widthSize = position.width / 2;
            float offSetSize = 10;
            float yOffset = FOLDOUT_HEIGHT + 1;
            int yOffsetMultipler = 1;

            SerializedProperty prefab = property.FindPropertyRelative("prefab");
            SerializedProperty useNewKey = property.FindPropertyRelative("useNewKey");
            SerializedProperty newKey = property.FindPropertyRelative("newKey");
            SerializedProperty startSize = property.FindPropertyRelative("startSize");
            SerializedProperty allowtoIncrement = property.FindPropertyRelative("allowtoIncrement");
            SerializedProperty disableAutoEnqueue = property.FindPropertyRelative("disableAutoEnqueue");
            
            Rect foldoutRect = new Rect(position.x, position.y, position.width, FOLDOUT_HEIGHT);

            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, new GUIContent(
                    prefab.objectReferenceValue != null ?
                    (prefab.objectReferenceValue as GameObject).name : "Empty"
                ));

            if (property.isExpanded)
            {
                Vector2 drawerPosition = new Vector2 (position.x, position.y + yOffset);
                Vector2 drawerPositionWithXOffset = new Vector2 (position.x + widthSize + offSetSize, position.y + yOffset);
                Vector2 drawerSize = new Vector2 (widthSize, FOLDOUT_HEIGHT);

                DrawProperty(prefab, "Prefab", drawerPosition, drawerSize);
                DrawProperty(useNewKey, "Use New Key", drawerPositionWithXOffset, drawerSize);

                if (useNewKey.boolValue)
                {
                    drawerPosition.y = position.y + yOffset * ++yOffsetMultipler;
                    drawerPositionWithXOffset.y = drawerPosition.y;

                    DrawProperty(newKey, "New key", drawerPosition, drawerSize);
                }

                drawerPosition.y = position.y + yOffset * ++yOffsetMultipler;
                drawerPositionWithXOffset.y = drawerPosition.y;

                DrawProperty(startSize, "Start Size", drawerPosition, drawerSize);
                DrawProperty(allowtoIncrement, "Allow Growth", drawerPositionWithXOffset, drawerSize);

                drawerPosition.y = position.y + yOffset * ++yOffsetMultipler;

                DrawProperty(disableAutoEnqueue, "Disable Auto Enqueue", drawerPosition, drawerSize);
            }

            EditorGUI.EndProperty();
        }

        private GUIContent LabelAdjustments(string labelText)
        {
            GUIContent newLabel = new GUIContent(labelText);
            float newSize = newLabel.text.Length * 8f;
            EditorGUIUtility.labelWidth = newSize < 80 ? 80 : newSize;
            
            return newLabel;
        }

        private void DrawProperty(SerializedProperty propertyToDraw, string labelText, Vector2 newRectPosition, Vector2 newRectSize)
        {
            Rect rectPosition = new Rect(newRectPosition, newRectSize);
            EditorGUI.PropertyField(rectPosition, propertyToDraw, LabelAdjustments(labelText));
        }
        
    }
}
