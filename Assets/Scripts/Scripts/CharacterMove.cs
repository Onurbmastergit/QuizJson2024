using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform moveTarget;
    Vector3 lastTile;

    public LayerMask wallCollider;

    private void Start()
    {
        moveTarget.parent = null;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, moveSpeed * Time.deltaTime);

        if (!CaseManager.Instance.canMove) return;

        if (Vector3.Distance(transform.position, moveTarget.position) <= .05f)
        {
            bool hitHorizontal = false;
            bool hitVertical = false;

            float posHorizontal = 0;
            float posVertical = 0;

            if (Math.Abs(Input.GetAxisRaw("Horizontal")) == 1)
            {
                if (!Physics2D.OverlapCircle(moveTarget.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, wallCollider))
                {
                    hitHorizontal = true;
                    posHorizontal = Input.GetAxisRaw("Horizontal");
                }
            }

            if (Math.Abs(Input.GetAxisRaw("Vertical")) == 1)
            {
                if (!Physics2D.OverlapCircle(moveTarget.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, wallCollider))
                {
                    hitVertical = true;
                    posVertical = Input.GetAxisRaw("Vertical");
                }
            }

            if (hitHorizontal && hitVertical)
            {
                if (!Physics2D.OverlapCircle(moveTarget.position + new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f), .2f, wallCollider))
                {
                    lastTile = moveTarget.position;
                    moveTarget.position += new Vector3(posHorizontal, posVertical, 0f);
                }
            } else
            {
                lastTile = moveTarget.position;
                moveTarget.position += new Vector3(posHorizontal, posVertical, 0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        moveTarget.position = lastTile;
    }
}
