﻿/*
 * Author: Shon Verch
 * File Name: RoomEditor.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 1/13/2017
 * Description: Room component inspector.
 */

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Room component inspector.
/// </summary>
[CustomEditor(typeof(Room), true)]
public class RoomEditor : Editor
{
    /// <summary>
    /// The <see cref="GUIContent"/> for the edit bounds button.
    /// </summary>
    private static GUIContent EditModeButton => EditorGUIUtility.IconContent("EditCollider");

    /// <summary>
    /// The <see cref="BoxBoundsHandle"/> for editing room bounds.
    /// </summary>
    private BoxBoundsHandle boxBoundsHandle;
    
    /// <summary>
    /// Indicates whether edit mode is enabled. 
    /// When it is enabled, the user can change the bounds of the room.
    /// </summary>
    private bool canEditBounds;

    /// <summary>
    /// The <see cref="SerializedPropertyManager"/> for this <see cref="SerializedObject"/>.
    /// </summary>
    private SerializedPropertyManager propertyManager;

    /// <summary>
    /// Called when the editor is enabled.
    /// </summary>
    private void OnEnable()
    {
        // Initialize the box bound handle with the appropriate axes.
        boxBoundsHandle = new BoxBoundsHandle
        {
            axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y
        };

        EditMode.onEditModeStartDelegate += (editor, mode) => canEditBounds = true;
        EditMode.onEditModeEndDelegate += editor => canEditBounds = false;

        boxBoundsHandle.handleColor = Color.yellow;
        boxBoundsHandle.wireframeColor = Color.green;

        propertyManager = new SerializedPropertyManager(serializedObject);
    }

    /// <inheritdoc />
    /// <summary>
    /// Draw the inspector.
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // This initializes the edit mode button and hook into the editor delegate for changing the scene view handle.
        // When the button is pressed, the position handle in the viewport is hidden.
        EditMode.DoEditModeInspectorModeButton(EditMode.SceneViewEditMode.Collider, "Edit Bounds", EditModeButton, () => new Bounds(boxBoundsHandle.center, boxBoundsHandle.size), this);
        propertyManager["size"].vector2IntValue = EditorGUILayout.Vector2IntField(new GUIContent("Size"), propertyManager["size"].vector2IntValue);

        // Readonly centre field.
        GUI.enabled = false;
        EditorGUILayout.Vector3Field(new GUIContent("Centre"), ((Room)target).transform.position);
        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Draw the scene GUI.
    /// </summary>
    private void OnSceneGUI()
    {
        Room room = (Room)target;
        SerializedProperty sizeProperty = serializedObject.FindProperty("size");

        using (new Handles.DrawingScope(GetGridViewMatrix(room.transform)))
        {
            if (canEditBounds)
            {
                boxBoundsHandle.size = (Vector2)sizeProperty.vector2IntValue;
                boxBoundsHandle.center = Vector3.zero;

                EditorGUI.BeginChangeCheck();
                boxBoundsHandle.DrawHandle();
                if (!EditorGUI.EndChangeCheck()) return;

                Undo.RecordObject((Room)target, $"Modify {ObjectNames.NicifyVariableName(target.GetType().Name)}");

                Vector3Int handleSize = boxBoundsHandle.size.FloorToInt();
                sizeProperty.vector2IntValue = new Vector2Int(handleSize.x, handleSize.y);
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                // Render the bounds of the room without any modification handles.
                Vector2 bottomLeft = new Vector2(-sizeProperty.vector2IntValue.x / 2f, -sizeProperty.vector2IntValue.y / 2f);
                Vector2 bottomRight = new Vector2(sizeProperty.vector2IntValue.x / 2f, -sizeProperty.vector2IntValue.y / 2f);

                Vector2 topLeft = new Vector2(-sizeProperty.vector2IntValue.x / 2f, sizeProperty.vector2IntValue.y / 2f);
                Vector2 topRight = new Vector2(sizeProperty.vector2IntValue.x / 2f, sizeProperty.vector2IntValue.y / 2f);

                Handles.DrawLine(bottomLeft, bottomRight);
                Handles.DrawLine(bottomLeft, topLeft);
                Handles.DrawLine(topLeft, topRight);
                Handles.DrawLine(topRight, bottomRight);
            }
        }
    }
        
    /// <summary>
    /// Retrieves <see cref="Matrix4x4"/> that is the grid view matrix.
    /// Rotates the matrix by 90 degrees on the x-axis so that it is top-down.
    /// </summary>
    private static Matrix4x4 GetGridViewMatrix(Transform gridTransform)
    {
        Vector3 rotation = new Vector3(90, 0, 0);
        Vector3 position = gridTransform.position;

        return Matrix4x4.Translate(position) * Matrix4x4.Rotate(gridTransform.rotation * Quaternion.Euler(rotation));
    }
}