using UnityEngine;
using Cinemachine;
 
/// <summary>
/// An add-on module for Cinemachine Virtual Camera which post-processes
/// the final position of the virtual camera.
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
public class CameraOffsetter : CinemachineExtension
{
    [Tooltip("How much to offset the camera, in camera-local coords")]
    public Vector3 m_Offset = Vector3.zero;
    protected override void PostPipelineStageCallback (
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (enabled && stage == CinemachineCore.Stage.Body)
            state.PositionCorrection += state.RawOrientation * m_Offset;
    }
}