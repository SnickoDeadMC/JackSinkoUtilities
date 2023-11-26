using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace JacksUtils
{
    [Serializable]
    public class AddressableSceneReference : ISerializationCallbackReceiver
    {
        
#if UNITY_EDITOR
        [SerializeField] private AssetReferenceT<SceneAsset> scene;
#endif

        [SerializeField, ReadOnly] private string sceneName;

        public string SceneName => sceneName;

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (scene == null || scene.editorAsset == null)
                sceneName = null;
            else sceneName = scene.editorAsset.name;
#endif
        }

        public void OnAfterDeserialize()
        {

        }
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AddressableSceneReference))]
    public class AddressableSceneReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);

            float halfWidth = position.width * 0.5f;

            Rect sceneRect = new Rect(position.x, position.y, halfWidth, position.height);
            SerializedProperty sceneProperty = property.FindPropertyRelative("scene");
            EditorGUI.PropertyField(sceneRect, sceneProperty, GUIContent.none);
            
            Rect sceneNameRect = new Rect(position.x + halfWidth, position.y, halfWidth, position.height);
            SerializedProperty sceneNameProperty = property.FindPropertyRelative("sceneName");
            EditorGUI.PropertyField(sceneNameRect, sceneNameProperty, GUIContent.none);
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
#endif
}