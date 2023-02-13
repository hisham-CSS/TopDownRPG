using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap colTilemap;
    private Player input;

    Vector3 endPos;
    bool isMoving = false;

    private void Awake()
    {
        input = new Player();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = groundTilemap.GetCellCenterWorld(new Vector3Int(0, 0, 0));
    }


    private void FixedUpdate()
    {
        if (isMoving)
            transform.position = Vector3.Lerp(transform.position, endPos, 0.1f);

        if (Vector3.Distance(transform.position, endPos) < 0.05f)
        {
            transform.position = endPos;
            isMoving = false;
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPos = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        
        if (!groundTilemap.HasTile(gridPos) || colTilemap.HasTile(gridPos))
            return false;

        return true;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (input.Main.Movement.IsPressed() && !isMoving)
        {
            Vector2 direction = input.Main.Movement.ReadValue<Vector2>();
            if (direction.x != 0 && direction.y != 0)
                return;

            if (CanMove(direction))
            {
                endPos = transform.position + (Vector3)direction;
                isMoving = true;
            }
        }
    }
}
