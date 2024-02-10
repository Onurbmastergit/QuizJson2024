using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform moveTarget;

    public LayerMask wallCollider;

    private void Start()
    {
        moveTarget.parent = null;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, moveSpeed * Time.deltaTime);

        if (!GameManager.Instance.canMove) return;

        if (Vector3.Distance(transform.position, moveTarget.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(moveTarget.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, wallCollider))
                {
                    moveTarget.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(moveTarget.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, wallCollider))
                {
                    moveTarget.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        moveTarget.position -= new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
        transform.position = moveTarget.position;
    }
}
