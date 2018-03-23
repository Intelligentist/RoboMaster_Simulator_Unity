#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace MK.Glow
{
    [CustomEditor(typeof(MKGlowFree))]
    public class MKGlowFreeEditor : Editor
    {
        private const string m_Style = "box";
        private ColorPickerHDRConfig colorPickerHDRConfig = new ColorPickerHDRConfig(0f, 99f, 1 / 99f, 3f);
        private static GUIContent glowTintLabel = new GUIContent("Glow Tint", "The glows coloration in full screen mode(only FullscreenGlowType)");

        private SerializedProperty glowType;
        private SerializedProperty samples;
        private SerializedProperty blurSpread;
        private SerializedProperty blurIterations;
        private SerializedProperty glowIntensity;
        private SerializedProperty glowTint;
        private SerializedProperty glowLayer;

        [MenuItem("Window/MKGlow/Add MKGlow To Selection")]
		private static void AddMKGlowToObject()
		{
			foreach (GameObject obj in Selection.gameObjects)
			{
				if (obj.GetComponent<MKGlowFree>() == null)
					obj.AddComponent<MKGlowFree>();
			}
		}

        private void OnEnable()
        {
            samples = serializedObject.FindProperty("samples");
            blurSpread = serializedObject.FindProperty("blurSpread");
            blurIterations = serializedObject.FindProperty("blurIterations");
            glowIntensity = serializedObject.FindProperty("glowIntensity");
            glowTint = serializedObject.FindProperty("glowTint");
            glowType = serializedObject.FindProperty("glowType");
            glowLayer = serializedObject.FindProperty("glowLayer");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(glowType);

            if (glowType.enumValueIndex == 0)
            {
                EditorGUILayout.PropertyField(glowLayer);
            }
            EditorGUILayout.Slider(blurSpread, 0.05f, 1.0f, "Blur Spread");
            EditorGUILayout.IntSlider(blurIterations, 0, 10, "Blur Iterations");
            EditorGUILayout.IntSlider(samples, 3, 8, "Blur Samples");
            EditorGUILayout.Slider(glowIntensity, 0f, 1f, "Glow Intensity");

            glowTint.colorValue = EditorGUILayout.ColorField(glowTintLabel, glowTint.colorValue, false, false, false, colorPickerHDRConfig);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif