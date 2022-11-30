using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] GameObject[] checkers;
    [SerializeField] float rayDistance;
    [SerializeField] bool isGrounded;
    [SerializeField] int maxJumpCount = 1;
    int howManyCanJump;
    public int HowManyCanJump { get { return howManyCanJump; } set { howManyCanJump -= value; howManyCanJump = Mathf.Clamp(howManyCanJump, 0, maxJumpCount); } }
    public bool IsGrounded => isGrounded;
    public static GroundCheck Instance;
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }

    private void Check()
    {
        foreach(var checker in checkers)
        {
            //Debug.DrawLine(checker.transform.position, Vector3.down * rayDistance, Color.red);
            RaycastHit hit;
            if(Physics.Raycast(checker.transform.position, Vector3.down, out hit, rayDistance))
            {
                if(hit.collider != null)
                {
                    howManyCanJump++;
                    howManyCanJump = Mathf.Clamp(howManyCanJump, 0, maxJumpCount);
                    isGrounded = true; return;
                }
            }
        }
        isGrounded = false;
    }
}
