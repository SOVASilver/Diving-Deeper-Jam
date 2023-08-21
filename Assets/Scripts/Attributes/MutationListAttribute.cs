using UnityEditor;
using UnityEngine;

public class MutationListAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(MutationListAttribute))]
public class MutationListDrawer : PropertyDrawer {
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		GUI.enabled = false;
		GUI.color = new Color(255, 255, 255, 255);
		EditorGUI.PropertyField(position, property, label);
		GUI.enabled = true;
	}
}