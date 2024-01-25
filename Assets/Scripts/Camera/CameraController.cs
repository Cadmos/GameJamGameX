using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    


    [SerializeField] private GameObject _cinemachineTargetObject;
    [SerializeField] private float _cinemachineTargetX;
    [SerializeField] private float _cinemachineTargetY;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    

    public void Look(Vector2 lookDirection, float lookSpeed)
    {
        _cinemachineTargetX += -lookDirection.y * lookSpeed * Time.deltaTime;
        _cinemachineTargetY += lookDirection.x * lookSpeed * Time.deltaTime;
        
    }
    /*
    public void SetActiveCamera(CinemachineVirtualCamera activeCamera)
    {
        foreach (var camera in cameras)
        {
            camera.Priority = (camera == activeCamera) ? 1 : 0;
        }
    }
    */

    private float CameraClampAngle(float angle, float angleMin, float angleMax)
    {
        if(angle < -360) angle += 360;
        if(angle > 360) angle -= 360;
        return Mathf.Clamp(angle, angleMin, angleMax);
    }
}
