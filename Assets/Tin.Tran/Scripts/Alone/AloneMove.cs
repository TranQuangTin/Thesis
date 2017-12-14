using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AloneMove : MonoBehaviour
{
    public float speed = 1f;
    public float rotatespeed = 3f;
    Transform MainCamera;

    public Rigidbody rigidbody;

    private void Start()
    {
        if (Preload1.Global == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }
        rigidbody = gameObject.GetComponent<Rigidbody>();
        MainCamera = Camera.main.transform;
        MainCamera.SetParent(gameObject.transform);
        MainCamera.localRotation = Quaternion.Euler(18, 0, 0);
        MainCamera.localPosition = new Vector3(0, 4, -5);
        speed = Map_Manager.Global.level.Speed;
        rotatespeed = Map_Manager.Global.level.RotateSpeed;
    }
    private void Update()
    {
        Move(Input.GetAxis("Vertical"));
        Turn(Input.GetAxis("Horizontal"));
    }
    private void Move(float value)
    {
        // tính độ di chuyển từ value của input
        Vector3 movement = transform.forward * value * speed * Time.deltaTime;
        // cộng vị trí hiện tại với độ di chuyển để có được vị trí mới
        Vector3 target = rigidbody.position + movement;
        // áp dụng vị trí cho rigidbody
        rigidbody.MovePosition(target);
    }
    private void Turn(float value)
    {
        //tính độ xoay từ input
        float turn = value * rotatespeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        // áp dụng độ xoay vào rigidbody
        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);
    }
    public void ReturnCamera()
    {
        MainCamera.SetParent(transform.parent);
    }
}
