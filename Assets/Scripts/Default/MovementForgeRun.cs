using UnityEngine;
using System;
using ZPackage;

///<Summary>Forge Run Controller 1<Summary>
public class MovementForgeRun : Mb
{
    public Player player;
    bool ControlAble;
    [SerializeField] float MaxSpeed = 10;
    [SerializeField] Transform targetPos;
    [SerializeField] Vector3? NoLookTaget;
    [SerializeField] bool shouldROt;
    [SerializeField] float targetX, speed = 0, lastFrameFingerPositionX, controlSensitivity, speedX;
    [SerializeField] float Xlimit = 6;
    Transform childModel;
    private void Start()
    {
        childModel = transform.GetChild(0);
    }
    void Update()
    {
        if (IsPlaying)
        {
            if (ControlAble)
            {
                transform.position += Vector3.forward * speed * Time.fixedDeltaTime;

                if (IsDown)
                {
                    LastFrameFingerPos();
                }
                // && transform.position.x >= -Xlimit && transform.position.x <= Xlimit
                if (IsClick && !shouldROt && Mathf.Abs(transform.position.x) <= Xlimit)
                {
                    targetX += (Input.mousePosition.x - lastFrameFingerPositionX) * controlSensitivity;

                    // LastFrameFingerPos();

                    transform.position = new Vector3(
                        Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * speedX),
                        transform.position.y, transform.position.z);

                    if (targetX - transform.position.x > 0.01f)
                        childModel.localRotation = Quaternion.Lerp(
                            childModel.localRotation, Quaternion.Euler(0, 24, 0), Time.fixedDeltaTime * 10);
                    else if (targetX - transform.position.x < -0.01f)
                        childModel.localRotation = Quaternion.Lerp(
                            childModel.localRotation, Quaternion.Euler(0, -24, 0), Time.fixedDeltaTime * 10);
                    else
                        childModel.localRotation = Quaternion.Lerp(
                            childModel.localRotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 5);

                }
                else if (!IsClick && !shouldROt)
                    childModel.localRotation = Quaternion.Lerp(childModel.localRotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 5);

                if (Mathf.Abs(transform.position.x) > Xlimit - 0.02f)
                {
                    transform.position = transform.position.ChangeX(Mathf.Clamp(transform.position.x, -Xlimit + 0.02f, Xlimit - 0.02f));
                }
                // else if (transform.position.x >= Xlimit)
                // {
                //     transform.position.ChangeX(Xlimit - 0.02f);
                // }
            }
            else if (targetPos)
            {
                ForwardMove(targetPos.position);
                childModel.localRotation = Quaternion.Lerp(childModel.localRotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 5);
                SetTargetX();
            }
            else if (NoLookTaget != null)
            {
                MoveNoLook();
                childModel.localRotation = Quaternion.Lerp(childModel.localRotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 5);
                SetTargetX();
            }
            LastFrameFingerPos();
        }
    }

    public void LastFrameFingerPos()
    {
        lastFrameFingerPositionX = Input.mousePosition.x;
    }
    public void SetTargetX()
    {
        targetX = transform.position.x;
    }

    internal void SetSpeedDirect(float value)
    {
        speed = value;
    }

    public void SetSpeed(float percent)
    {
        speed = MaxSpeed * percent;
 
    }
    public float GetSpeed()
    {
        return speed;
    }
    public void GoToPosition(Vector3 _target, bool noLook, float speedPercent = 1, Action afterAction = null)
    {
        SetSpeed(speedPercent);
        targetPos = null;
        NoLookTaget = _target;
        CancelDistance = 0.1f;
        afterGoAction = afterAction;
    }
    public void GoToPosition(Transform _target, float _cancelDistance = 0.1f, float speedPercent = 1, Action afterAction = null)
    {
        SetSpeed(speedPercent);
        targetPos = _target;
        CancelDistance = _cancelDistance;
        afterGoAction = afterAction;
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
        NoLookTaget = null;
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

    public void ClampPosition()
    {
        if (transform.position.x <= -6.8f || transform.position.x >= 6.8f)
        {
            var pos = rb.position;
            pos.x = Mathf.Clamp(rb.position.x, -6.8f, 6.8f);
            rb.position = pos;
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
    float CancelDistance = 0.1f;
    public void ForwardMove(Vector3 _targetPos)
    {
        _targetPos.y = 0;
        Vector3 lookTaget = _targetPos;
        lookTaget.y = 0;
        transform.LookAt(lookTaget);
        // Vector3 forwardMove = Vector3.forward * Speed * Time.fixedDeltaTime;
        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        // rb.MovePosition(transform.position + forwardMove);
        rb.position += forwardMove;
        float distance = Vector3.Distance(transform.position, _targetPos);
        if (distance < CancelDistance)
        {
            Cancel();
        }

        ClampPosition();
    }
    private void MoveNoLook()
    {
        rb.position = Vector3.MoveTowards(rb.position, NoLookTaget.Value, speed * Time.fixedDeltaTime);
        float distance = Vector3.Distance(transform.position, NoLookTaget.Value);
        if (distance < CancelDistance)
        {
            Cancel();
        }

    }
}
