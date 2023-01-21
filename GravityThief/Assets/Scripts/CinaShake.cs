using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinaShake : MonoBehaviour
{
    public static CinaShake Instance {get; private set;}
    private CinemachineVirtualCamera vCam;

    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingInten;

    private void Awake() {
        Instance = this;
        vCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time){
        print("test");
        CinemachineBasicMultiChannelPerlin vCamPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        vCamPerlin.m_AmplitudeGain = intensity;
        startingInten = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void Update() {
        if(shakeTimer > 0){
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin vCamPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            vCamPerlin.m_AmplitudeGain = Mathf.Lerp(startingInten, 0f, (1-(shakeTimer/shakeTimerTotal)));
        }
    }
}
