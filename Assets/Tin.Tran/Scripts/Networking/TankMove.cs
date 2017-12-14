using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankMove : NetworkBehaviour
{
    public float speed = 1f;
    public float rotatespeed = 3f;
    Transform MainCamera;

    public Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        MainCamera = Camera.main.transform;
        if (!isLocalPlayer) return;
        MainCamera.SetParent(gameObject.transform);
        MainCamera.localRotation = Quaternion.Euler(18, 0, 0);
        MainCamera.localPosition = new Vector3(0, 4, -5);
    }
    private void Update()
    {
        if (!isLocalPlayer) return;
        Move(Input.GetAxis("Vertical"));
        Turn(Input.GetAxis("Horizontal"));
    }

    private void Move(float value)
    {
        Vector3 movement = transform.forward * value * 4 * Time.deltaTime;
        Vector3 target = rigidbody.position + movement;
        rigidbody.MovePosition(target);
        // MainCamera.position = new Vector3(target.x, target.y + 5.5f, target.z - 9);
    }


    private void Turn(float value)
    {
        float turn = value * 70 * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);
    }
    public void ReturnCamera()
    {
        MainCamera.SetParent(transform.parent);
    }
}
