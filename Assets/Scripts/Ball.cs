using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static List<Ball> balls = new List<Ball>();

    public Vector3 lastVel;
    public bool isContactBall;
    private void OnEnable()
    {
        balls.Add(this);    
    }

    private void OnDisable()
    {
        balls.Remove(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isContactBall = collision.other.GetComponent<Ball>() != null;
       
    }
}
