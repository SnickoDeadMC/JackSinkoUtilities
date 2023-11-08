using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JacksUtils.Editor
{
    public class MultipleEnumAttribute : PropertyAttribute
    {
        public MultipleEnumAttribute()
        {

        }
    }

    [CustomPropertyDrawer(typeof(MultipleEnumAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Enum)
            {
                property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use MultipleEnum with enum types");
            }
        }
    }
}