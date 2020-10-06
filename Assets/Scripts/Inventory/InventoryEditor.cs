using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
    private SerializedProperty itemImagesProperty;
    private SerializedProperty itemsProperty;
    private readonly bool[] showItemSlots = new bool[Inventory.ItemSlotsAmount];

    private const string InventoryPropertyItemImagesName = "itemImages";
    private const string InventoryPropertyItemsName = "items";

    private void OnEnable()
    {
        itemImagesProperty = serializedObject.FindProperty(InventoryPropertyItemImagesName);
        itemsProperty = serializedObject.FindProperty(InventoryPropertyItemsName);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        for (var i = 0; i < Inventory.ItemSlotsAmount; i++)
            ItemSlotGUI(i);
        serializedObject.ApplyModifiedProperties();
    }

    private void ItemSlotGUI(int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        showItemSlots[index] = EditorGUILayout.Foldout(showItemSlots[index], "Item slot " + index);
        if (showItemSlots[index])
        {
            EditorGUILayout.PropertyField(itemImagesProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(itemsProperty.GetArrayElementAtIndex(index));
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
