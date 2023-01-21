using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCams : MonoBehaviour
{
    [SerializeField] GameObject virtualCam;
    [SerializeField] Player playerObj;
    
    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.CompareTag("Player") && !other.isTrigger){
            virtualCam.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {

        if(other.gameObject.CompareTag("Player") && !other.isTrigger){
            virtualCam.SetActive(false);
        }
    } 

}
