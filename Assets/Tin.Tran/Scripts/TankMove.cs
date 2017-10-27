using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankMove : MonoBehaviour
{
    public float speed = 1f;
    public float rotatespeed = 3f;


    public Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //multy
        //if (!isLocalPlayer) return;
        Move(Input.GetAxis("Vertical"));
        Turn(Input.GetAxis("Horizontal"));
    }

    private void Move(float value)
    {
        Vector3 movement = transform.forward * value * 4 * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + movement);
    }


    private void Turn(float value)
    {
        float turn = value * 70 * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);
    }
}
