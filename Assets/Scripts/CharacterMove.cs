using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float walkSpeed, jumpSpeed, mouseSensitivity;
    [SerializeField] GameObject playerCamera;
    float camVerticalAngle = 0f;
    Rigidbody rb;
    bool jump;
    public bool freeze;
    public static CharacterMove Instance;
    public bool CanJump => GroundCheck.Instance.HowManyCanJump > 0;

    void Start()
    {
        Instance = this;
        rb = this.gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanJump)
        {
            Hook.Instance.ChangeTarget(Vector3.zero);
            Hook.Instance.HookingFalse();
            rb.isKinematic = false;
            jump = true;
            GroundCheck.Instance.HowManyCanJump = 1;
            Debug.Log(GroundCheck.Instance.HowManyCanJump);
        }
    }
    void FixedUpdate(){
        if(!freeze) Move();
        Look();
        if(jump && !freeze)
        {
            Jump();
            jump = false;
        }
    }
    

    private void Look()
    {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");
        transform.Rotate(new Vector3(0, lookX, 0) * mouseSensitivity, Space.Self);
        camVerticalAngle -= lookY * mouseSensitivity;
        camVerticalAngle = Mathf.Clamp(camVerticalAngle, -89f, 89f);
        playerCamera.transform.localEulerAngles = new Vector3(camVerticalAngle, 0, 0);
    }

    void Move(){
        var speed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)){
            speed *= 2f;
        }
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        rb.MovePosition(transform.position + ((transform.forward * z) + (transform.right * x)) * speed * Time.deltaTime);
    }
    void Jump(){
        var velocity = new Vector3(0, jumpSpeed ,0);
        rb.AddForce(velocity * Time.deltaTime);
    }
    

}
