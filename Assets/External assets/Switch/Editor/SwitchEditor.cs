using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(Switch), true), CanEditMultipleObjects]
    public class SwitchEditor : SelectableEditor
    {
        SerializedProperty m_OnValueChangedProperty;
        SerializedProperty m_TransitionProperty;
        SerializedProperty m_GraphicProperty;
        SerializedProperty m_IsOnProperty;
        SerializedProperty m_PanelColor;
        SerializedProperty m_textColor;
        SerializedProperty m_TransitionDuration;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_TransitionProperty = serializedObject.FindProperty("toggleTransition");
            m_GraphicProperty = serializedObject.FindProperty("graphic");
            m_IsOnProperty = serializedObject.FindProperty("m_IsOn");
            m_OnValueChangedProperty = serializedObject.FindProperty("onValueChanged");
            m_PanelColor = serializedObject.FindProperty("PanelColor");
            m_textColor = serializedObject.FindProperty("m_textColor");
            m_TransitionDuration = serializedObject.FindProperty("TransitionDuration");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.PropertyField(m_IsOnProperty);

            EditorGUILayout.PropertyField(m_textColor, true);
            EditorGUILayout.PropertyField(m_PanelColor, true);
            EditorGUILayout.PropertyField(m_TransitionDuration);

            EditorGUILayout.PropertyField(m_TransitionProperty);
            EditorGUILayout.PropertyField(m_GraphicProperty);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_OnValueChangedProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}