using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Shell : MonoBehaviour
{
    public float Damage;
    private bool isInit;
    public Vector3 Size;
    public Vector2Int CurrentPosition { get; set; }
    public TileBase CurrentTile { get; set; }
    public int Timer = 500;
    protected Area area;
    KeyCode keyCode;
    public bool IsKeyW { get; set; }
    public bool IsKeyD { get; set; }
    public bool IsKeyA { get; set; }
    public bool IsKeyS { get; set; }
    public virtual void Collision()
    {
        Del();
    }

    public void Init(Area area, KeyCode keyCode)
    {
        this.area = area;
        this.keyCode = keyCode;
        CurrentPosition = area.UserService.CurrentPostition;
        gameObject.transform.position = new Vector3(CurrentPosition.x + 0.5f, CurrentPosition.y + 0.5f, gameObject.transform.position.z);
        gameObject.transform.localScale = Size;
        print("init");
        IsKeyW = keyCode == KeyCode.UpArrow;
        IsKeyD = keyCode == KeyCode.RightArrow;
        IsKeyA = keyCode == KeyCode.LeftArrow;
        IsKeyS = keyCode == KeyCode.DownArrow;

        upDt = DateTime.Now;
        isInit = true;
    }

    private DateTime upDt;
    private void FixedUpdate()
    {
        if (isInit)
        {
            if (DateTime.Now - upDt >= new TimeSpan(0, 0, 0, 0, Timer))
            {
                if (IsKeyS || IsKeyW)
                {
                    LineMotion(IsKeyW, IsKeyS, (p, c) => new Vector2Int(p.x, p.y + c * 1));
                }
                else if (IsKeyA || IsKeyD)
                {
                    LineMotion(IsKeyD, IsKeyA, (p, c) => new Vector2Int(p.x + c * 1, p.y));
                }
                upDt = DateTime.Now;
            }
        }
    }

    Vector3? destination = null;
    private void Update()
    {
        if (!destination.HasValue || destination == gameObject.transform.position)
            return;


        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, destination.Value, 0.1f);
    }

    private void LineMotion(bool key1, bool key2, Func<Vector2Int, int, Vector2Int> vectorFunc)
    {
        var c = key1 ? 1 : key2 ? -1 : 0;

        var lastPosition = CurrentPosition;
        var lastTile = CurrentTile;
        print(CurrentPosition);
        CurrentPosition = vectorFunc(CurrentPosition, c);
        CurrentTile = area.Tilemap.GetTile((Vector3Int)CurrentPosition);

        if (!area.BlockList.FirstOrDefault(p => p.Tile == CurrentTile)?.IsMotion ?? true)
        {
            print("de");
            Collision();
            return;
        }
        print(CurrentPosition);


        destination = new Vector3(CurrentPosition.x + 0.5f, CurrentPosition.y + 0.5f,
            gameObject.transform.position.z);


        var mob = area.MobsService.mobs.FirstOrDefault(p => p.CurrentPoint == CurrentPosition);
        if (mob != null)
        {
            mob.Hit(Damage);
            Collision();
        }

        var isDamageUser = area.UserService.CurrentPostition == CurrentPosition;
        if (isDamageUser)
        {
            area.UserService.Hit(Damage);
            Collision();
        }
    }

    private void Del()
    {
        if(gameObject != null)
            Destroy(gameObject);
    }
}
