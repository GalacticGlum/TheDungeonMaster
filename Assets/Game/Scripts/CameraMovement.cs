﻿/*
 * Author: Shon Verch
 * File Name: CameraMovement.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 1/21/2018
 * Description: Manages all camera movement.
 */

using UnityEngine;

/// <summary>
/// Manages all camera movement.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    public float RotationDuration => rotationDuration;

    [SerializeField]
    private float transitionDuration = 1f;
    [SerializeField]
    private float rotationDuration = 1f;
    [SerializeField]
    private float cameraHeight = 9f;
    [SerializeField]
    private float cameraDistance = 4f;
    [SerializeField]
    private float cameraAngle = 70f;

    private RoomController roomController;
    private bool roomHasChanged;

    private LerpInformation<Vector3> cameraTransitionLerpInformation;
    private LerpInformation<Quaternion> cameraRotateLerpInformation;

    private Quaternion targetCameraRotation;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        roomController = ControllerDatabase.Get<RoomController>();

        transform.position = new Vector3(0, cameraHeight, -cameraDistance);
        transform.rotation = Quaternion.Euler(cameraAngle, 0, 0);
        roomController.CurrentRoomChanged += OnCurrentRoomChanged;
        targetCameraRotation = Quaternion.Euler(cameraAngle, 0, 0);
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        if (cameraRotateLerpInformation != null)
        {
            transform.rotation = cameraRotateLerpInformation.Step(Time.deltaTime);
            transform.position = CalculateCameraPosition(roomController.CurrentRoom);
            return;
        }

        if (roomHasChanged)
        {
            roomHasChanged = false;
            Vector3 destination = CalculateCameraPosition(roomController.CurrentRoom);
            cameraTransitionLerpInformation = new LerpInformation<Vector3>(transform.position, destination, transitionDuration, GradualCurve.Interpolate);
            cameraTransitionLerpInformation.Finished += (obj, eventArgs) => cameraTransitionLerpInformation = null;
        }

        if (cameraTransitionLerpInformation == null) return;
        transform.position = cameraTransitionLerpInformation.Step(Time.deltaTime);
    }

    /// <summary>
    /// Event which is called when the current room (that the player is in) has changed.
    /// </summary>
    /// <param name="sender">The object which dispatched this event.</param>
    /// <param name="args">The arguments pertaining to the event.</param>
    private void OnCurrentRoomChanged(object sender, CurrentRoomChangedEventArgs args)
    {
        roomHasChanged = true;
    }

    /// <summary>
    /// Sets the camera rotation angle.
    /// </summary>
    public void SetRotation(float rotation)
    {
        targetCameraRotation = Quaternion.Euler(new Vector3(cameraAngle, rotation, 0));
        cameraRotateLerpInformation = new LerpInformation<Quaternion>(transform.rotation, targetCameraRotation, rotationDuration, GradualCurve.Interpolate);
        cameraRotateLerpInformation.Finished += (obj, eventArgs) => cameraRotateLerpInformation = null;
    }

    /// <summary>
    /// Calculates the position of the camera from the specified room centre.
    /// </summary>
    private Vector3 CalculateCameraPosition(Room room)
    {
        Vector3 transformDirection = transform.forward;
        transformDirection.y = 0;
        transformDirection.Normalize();
        transformDirection *= cameraDistance;
        return room.Centre + new Vector3(0, cameraHeight, 0) - transformDirection;
    }
}
