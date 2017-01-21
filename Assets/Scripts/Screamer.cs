using UnityEngine;

public class Screamer : MonoBehaviour
{

    public GameObject Scream;
    public bool IsScreaming;
    // become private
    public int ScreamStrength;

    public bool IsLocked;

    private bool IsDragging = false;
    private bool StartDragging = false;

    private float DragStartTime = 0;
    private SpriteRenderer Renderer;

    public Sprite[] Sprites;

    public Direction Direction;

    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
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
        if (Time.time - DragStartTime < 1)
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
            if (Time.time - DragStartTime > 1)
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