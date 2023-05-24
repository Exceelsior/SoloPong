using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCameraAxis : CinemachineExtension
{
    public bool m_LockXPosition = false;
    public bool m_LockYPosition = false;
    public bool m_LockZPosition = false;

    [Tooltip("Lock the camera's Z position to this value")]
    public float m_XPosition = 10;
    [Tooltip("Lock the camera's Z position to this value")]
    public float m_YPosition = 10;
    [Tooltip("Lock the camera's Z position to this value")]
    public float m_ZPosition = 10;



    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            if(m_LockXPosition) pos.x = m_XPosition;
            if(m_LockYPosition) pos.y = m_YPosition;
            if(m_LockZPosition) pos.z = m_ZPosition;
           
            state.RawPosition = pos;

        }
    }
}