using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public PlayerState playerState;
    
    private void OnTriggerEnter(Collider other)
    {
        var ball = other.GetComponent<Ball>();
        if(ball != null)
        {
            playerState.CaptureBall(ball);
        }   
    }
}
