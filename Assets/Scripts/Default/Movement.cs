using UnityEngine;
using System;
using ZPackage;


///<Summary>Catch Master 3D Controller<Summary>
public class Movement : Mb
{
    public AnimationController animationController;
    public float rotationSpeed = 10;

    public float Speed = 0;
    public bool ControlAble;
    [SerializeField] float MaxSpeed = 6;
    [SerializeField] Transform targetPos;

    private void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ControlAble && /* !IsFrontObject() && */ IsPlaying)
        {
            ForwardMove();
        }
        else if (targetPos != null)
        {
            ForwardMove(targetPos.position);
        }
    }
    void Update()
    {
        if (IsPlaying)
        {
            if (IsClick && ControlAble)
            {
                HorizontalControl();
            }
        }
    }
    public void SetSpeed(float percent)
    {
        Speed = MaxSpeed * percent;
        // animationController.SetSpeed(Speed / MaxSpeed);
    }
    public float GetSpeed()
    {
        return Speed;
    }
    public void GoToPosition(Transform _target, float _cancelDistance = 0.1f)
    {
        SetSpeed(1);
        targetPos = _target;
        CancelDistance = _cancelDistance;
    }
    public Action afterGoAction;
    public void GoToPosition(Vector3 _target, float _cancelDistance = 0.1f, Action afterAction = null)
    {
        SetSpeed(1);
        CancelDistance = _cancelDistance;
        GameObject Goto = new GameObject("Goto");
        Goto.transform.position = _target;
        targetPos = Goto.transform;
        afterGoAction = afterAction;
    }
    public void Cancel(bool cancelAfterInvoke = false)
    {
        if (targetPos && targetPos.name == "Goto")
        {
            Destroy(targetPos.gameObject);
        }
        targetPos = null;
        SetSpeed(0);
        if (cancelAfterInvoke)
        {
            afterGoAction = null;
        }
        afterGoAction?.Invoke();
    }
    public void SetControlAble(bool value)
    {
        ControlAble = value;
    }

    void ForwardMove()
    {

        // Vector3 forwardMove = Vector3.forward * Speed * Time.fixedDeltaTime;
        Vector3 forwardMove = transform.forward * Speed * Time.fixedDeltaTime;
        // rb.MovePosition(transform.position + forwardMove);
        rb.position += forwardMove;

        // transform.Translate(forwardMove);
        // var velocity =rb.velocity;
        // velocity.z = forwardMove.z * 60;
        // rb.velocity = velocity;
        ClampPosition();
    }
    float CancelDistance = 0.1f;
    public void ForwardMove(Vector3 _targetPos)
    {
        _targetPos.y = 0;
        Vector3 lookTaget = _targetPos;
        lookTaget.y = 0;
        transform.LookAt(lookTaget);
        // Vector3 forwardMove = Vector3.forward * Speed * Time.fixedDeltaTime;
        Vector3 forwardMove = transform.forward * Speed * Time.fixedDeltaTime;
        // rb.MovePosition(transform.position + forwardMove);
        rb.position += forwardMove;
        float distance = Vector3.Distance(transform.position, _targetPos);
        if (distance < CancelDistance)
        {
            Cancel();
        }
        if (transform.position.x <= -7.4f || transform.position.x >= 7.4f)
        {
            Cancel();
        }

        // transform.Translate(forwardMove);
        // var velocity =rb.velocity;
        // velocity.z = forwardMove.z * 60;
        // rb.velocity = velocity;
        ClampPosition();
    }

    public void ClampPosition()
    {
        if (transform.position.x <= -7.4f || transform.position.x >= 7.4f)
        {
            var pos = rb.position;
            pos.x = Mathf.Clamp(rb.position.x, -7.4f, 7.4f);
            rb.position = pos;
        }
    }

    public void HorizontalControl()
    {
        float HorizontalInput = Input.GetAxis("Mouse X");
        // var pos = rb.position;
        // pos.x = Mathf.Clamp(transform.position.x + horizontalSpeed * Time.fixedDeltaTime * HorizontalInput, -4.8f, 4.8f);

        // pos.x = rb.position.x + horizontalSpeed * Time.fixedDeltaTime * HorizontalInput;
        // rb.position = pos;

        Vector3 movementDirection = new Vector3(HorizontalInput, 0, 0);
        movementDirection.Normalize();
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            // pointer.transform.rotation = Quaternion.RotateTowards(pointer.transform.rotation, toRotation, rotationSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }

    bool IsGrounded()
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo, 1.05f);

        return hitInfo.collider != null;
    }
    bool IsOnAir()
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo, 2f);
        // print(hitInfo.collider == null);
        return hitInfo.collider == null;
    }

    Collider FrontRayCast(float length)
    {
        RaycastHit hitInfo;
        float rayCastHeight = 0.2f;
        for (int i = 0; i < 3; i++)
        {
            Physics.Raycast(transform.position + Vector3.up * rayCastHeight, transform.forward, out hitInfo, length);
            rayCastHeight += 0.7f;
            if (hitInfo.collider)
            {
                return hitInfo.collider;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        //IsGrounded ray
        Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 1.05f, Color.blue);
        //IsOnAir ray
        Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 2.0f, Color.cyan);
        //FrontRay
        Debug.DrawRay(transform.position + Vector3.up * 1.2f, transform.forward * 0.31f, Color.cyan);
    }
    // public void SetDefault()
    // {
    //     animationController.ResetAll();
    //     ControlAble = true;
    // }

}
