using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    Rigidbody rb;

    bool hanging;

    public float Jump { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    
    void Update()
    {
        PlayerLedgeGrab();

        if(Input.GetButtonDown("Jump") && Mathf.Approximately(rb.linearVelocity.y, 0))
        {
            if (hanging)
            {
                rb.useGravity = true;
                hanging = false;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, Jump, rb.linearVelocity.z);
            }
            else
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, Jump, rb.linearVelocity.z);
            }
                
        }
    }
    void PlayerLedgeGrab()
    {
        if (rb.linearVelocity.y < 0 && !hanging)
        {
            RaycastHit downHit;
            Vector3 lineDownStart = (transform.position + Vector3.up * 1.5f) + transform.forward;
            Vector3 lineDownEnd = (transform.position + Vector3.up * 0.7f) + transform.forward;
            Physics.Linecast(lineDownStart, lineDownEnd, out downHit, LayerMask.GetMask("Ground"));
            Debug.DrawLine(lineDownStart, lineDownEnd);

            if (downHit.collider != null)
            {
                RaycastHit fwdHit;
                Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
                Vector3 lineFwdEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward;
                Physics.Linecast(lineFwdStart, lineFwdEnd, out fwdHit, LayerMask.GetMask("Ground"));
                Debug.DrawLine(lineFwdStart, lineFwdEnd);

                if (fwdHit.collider != null)
                {
                    rb.useGravity = false;
                    rb.linearVelocity = Vector3.zero;

                    hanging = true;

                    Vector3 hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -1f;
                    hangPos += offset;
                    transform.position = hangPos;
                    transform.forward = -fwdHit.normal;
                }
            }
        }
    }

}
