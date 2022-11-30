using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    [SerializeField] float dashSpeed = 1f, dashDistance = 1f;
    Rigidbody rb;
    Vector3 dashTarget = Vector3.zero, dest = Vector3.zero;
    bool dashing, canDash;
    public static CombatMovement Instance;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl) && canDash){
            dashTarget = transform.position + (DashTarget() * dashDistance);
            dest = DashTarget();
            dashing = true;
        }
        if(dashing && dashTarget != Vector3.zero && dest != Vector3.zero){
            CanDash(false);
            var dist = Vector3.Distance(transform.position, dashTarget);
            Debug.Log("dist" + dist);
            if(dist > 0.5f){
                transform.position = Vector3.MoveTowards(transform.position, transform.position + dest, Time.deltaTime * dashSpeed);
            }else{
                dashTarget = Vector3.zero;
                dest = Vector3.zero;
                dashing = false;
            }
        }else{
            CanDash(true);
        }
    }
    Vector3 DashTarget(){
        var tar = Vector3.zero;
        if(Input.GetKey(KeyCode.A)){
            tar = -transform.right;
        }else if(Input.GetKey(KeyCode.D)){
            tar = transform.right;
        }else if(Input.GetKey(KeyCode.S)){
            tar = -transform.forward;
        }else{
            tar = transform.forward;
        }
        return tar;
    }
    public void DashingFalse(){
        dashing = false;
    }
    public void CanDash(bool a){
        canDash = a;
    }
}
