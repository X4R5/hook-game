using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float hookSpeed, hookDistance, targetDistance;
    [SerializeField] Transform hookPos, firePoint;
    [SerializeField] LayerMask layerMask;
    bool isHooking, hookDone = true;
    Vector3 target, velocityToSet;
    public GameObject cube; //silinecek
    Rigidbody rb;
    LineRenderer lr;
    public static Hook Instance;
    [SerializeField] float overshootYAxis;

    void Awake()
    {
        Instance = this;
        lr = firePoint.GetComponentInParent<LineRenderer>();
    }
    void Start()
    {
        lr.enabled = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, firePoint.position);
        DrawLine();
        if(Input.GetKeyDown(KeyCode.E) && GetRay() != Vector3.zero)
        {
            isHooking = true;
            //Debug.Log(GetRay());
            target = GetRay();
            cube.transform.position = target;
            //rb.isKinematic = true;
            CombatMovement.Instance.DashingFalse();
        }else if(Input.GetKeyDown(KeyCode.E) && isHooking && GetRay() == Vector3.zero)
        {
            target = Vector3.zero;
            rb.isKinematic = false;
            isHooking = true;
            CombatMovement.Instance.DashingFalse();
        }
        if(isHooking){
            if(hookDone) StartHook();
            CombatMovement.Instance.CanDash(false);
            CombatMovement.Instance.DashingFalse();
        }else{
            CombatMovement.Instance.CanDash(true);
        }

        //if (IsCloseToTarget())
        //{
        //    SetVelocity(rb.velocity / 10);
        //}

        //if(target != Vector3.zero)
        //{
        //    if (!IsCloseToTarget())
        //    {
        //        HookMove(target);
        //    }
        //    else
        //    {
        //        rb.isKinematic = false;
        //        target = Vector3.zero;
        //        isHooking = false;
        //    }
        //}
    }

    private void StartHook()
    {
        hookDone = false;
        lr.enabled = true;
        lr.SetPosition(1, target);
        CharacterMove.Instance.freeze = true;
        Invoke(nameof(ExecuteHook), 0.1f);
    }
    void ExecuteHook()
    {
        JumpToPositon();
    }
    void StopHook()
    {
        isHooking = false;
        hookDone = true;
        CharacterMove.Instance.freeze = false;
        lr.enabled = false;
    }

    private void DrawLine()
    {
        if(GetRay() != Vector3.zero) Debug.DrawLine(hookPos.position, GetRay(), Color.red);
    }
    Vector3 GetRay()
    {
        RaycastHit hit;
        if(Physics.Raycast(hookPos.position, hookPos.forward, out hit, hookDistance, layerMask)){
            if(hit.collider != null)
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            return Vector3.zero;
        }
    }
    void JumpToPositon()
    {
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float hookPointRelativeYPos = target.y - lowestPoint.y;
        float highestPoint = hookPointRelativeYPos + overshootYAxis;
        if (hookPointRelativeYPos < 0) highestPoint = overshootYAxis;
        velocityToSet = CalculateVelocity(transform.position, target, highestPoint);
        Debug.Log(velocityToSet);
        CharacterMove.Instance.freeze = true;
        Invoke(nameof(SetVelocity), 0.1f);
        Invoke(nameof(StopHook), 1f); //try to make it with distance
    }
    void SetVelocity()
    {
        rb.velocity = velocityToSet;
    }
    void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    //stolen code
    Vector3 CalculateVelocity(Vector3 startPoint, Vector3 endPoint, float height)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity));
        
        return velocityXZ + velocityY;
    }
    bool IsCloseToTarget()
    {
        return Mathf.Abs(target.x - this.transform.position.x) <= targetDistance && Mathf.Abs(target.y - this.transform.position.y) <= targetDistance && Mathf.Abs(target.z - this.transform.position.z) <= targetDistance;
    }
    public void HookingFalse(){
        isHooking = false;
    }
    public void ChangeTarget(Vector3 t){
        target = t;
    }
    
}
