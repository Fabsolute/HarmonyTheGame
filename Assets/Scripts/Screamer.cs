﻿using UnityEngine;
using System.Collections.Generic;
public class Screamer : InteractiveHarmonyMonoBehaviour
{
    public Scream Scream;
    public bool IsLocked;
    public bool IsStart;
    public float ScreamLength = 1;

    public Sprite[] Sprites;
    public Direction Direction;
    public List<InteractiveHarmonyMonoBehaviour> OtherInteractives = new List<InteractiveHarmonyMonoBehaviour>();

    private int ScreamStrength;
    private bool IsDragging = false;
    private bool StartDragging = false;

    private float DragStartTime = 0;
    private SpriteRenderer Renderer;

    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (IsStart)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                DoAction();
            }
        }
    }

    public override void DoAction()
    {
        if (!IsActionCompleted)
        {
            IsActionCompleted = true;
            MusicManager.Instance.PlayNext();
            PTweenManager.Instance.RoutineTo(ScreamLength, 1, 0, (callback) =>
               {
                   Scream.SetCutout(callback);
               },
            () =>
                {
                    CheckColliderOtherScreamers();
                }
            );
        }
    }

    private void CheckColliderOtherScreamers()
    {
        foreach (var interactive in OtherInteractives)
        {
            var is_blocked = false;
            var direction = (interactive.transform.position - transform.position);
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, direction.magnitude);
            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Box")
                    {
                        is_blocked = true;
                    }
                }
            }

            if (!is_blocked)
            {
                interactive.DoAction();
            }
        }
    }

    void OnMouseDown()
    {
        IsDragging = true;
        StartDragging = false;
        DragStartTime = Time.time;
    }

    void OnMouseUp()
    {
        IsDragging = false;
        if (Time.time - DragStartTime < 0.5f)
        {
            RotateCharacter();
        }
    }

    void OnMouseDrag()
    {
        if (StartDragging && IsDragging && !IsLocked)
        {
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var snapX = (int)(cursorPosition.x / GameManager.Instance.SnapX);
            var snapY = (int)(cursorPosition.y / GameManager.Instance.SnapY);
            transform.position = new Vector3(snapX * GameManager.Instance.SnapX, snapY * GameManager.Instance.SnapY);
        }
        else if (!StartDragging)
        {
            if (Time.time - DragStartTime > 0.5f)
            {
                StartDragging = true;
            }
        }
    }

    void RotateCharacter()
    {
        switch (Direction)
        {
            case Direction.Left:
                {
                    Direction = Direction.Down;
                }
                break;
            case Direction.Down:
                {
                    Direction = Direction.Right;
                }
                break;
            case Direction.Right:
                {
                    Direction = Direction.Up;
                }
                break;
            case Direction.Up:
                {
                    Direction = Direction.Left;
                }
                break;
        }
        Renderer.sprite = Sprites[(int)Direction];
        Scream.transform.parent.eulerAngles = new Vector3(Scream.transform.parent.eulerAngles.x,
            Scream.transform.parent.eulerAngles.y,
            90 * (int)Direction);
    }

}

public enum Direction
{
    Left,
    Down,
    Right,
    Up
}