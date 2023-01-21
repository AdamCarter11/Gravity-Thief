using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{   
    [SerializeField] float shakeMagnitude;
    [SerializeField] float dampeningSpeed;
    [HideInInspector] public Vector3 initialPos;
    float shakeDuration;
    //Transform transformVar;

    void Start()
    {
        /*
        if(transform == null){
            transformVar = GetComponent<Transform>();
        }
        */
        initialPos = transform.localPosition;
    }

    void Update()
    {
        if(shakeDuration > 0){
            transform.localPosition = initialPos + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampeningSpeed;
        }
        else{
            shakeDuration = 0f;
            //transform.localPosition = initialPos;
        }
    }
    public void TriggerShake(){
        shakeDuration = .5f;
    }
}
