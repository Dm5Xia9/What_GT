using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UserService
{
    public bool IsKeyW { get; set; }
    public bool IsKeyD { get; set; }
    public bool IsKeyA { get; set; }
    public bool IsKeyS { get; set; }
    public Vector3Int CurrentPostition { get; set; }
    public TileBase CurrentTile { get; set; }

    public Vector3Int StartPosition { get; set; }
    public TileBase TileUser { get; set; }
    public int Step { get; set; }

    private Tilemap area;
    private List<Block> blocks;
    public UserService(UserOptions userOptions, Tilemap area, List<Block> blocks)
    {
        this.blocks = blocks;
        StartPosition = userOptions.StartPosition;
        TileUser = userOptions.TileUser;
        Step = userOptions.Step;
        this.area = area;

        CurrentPostition = StartPosition;
        CurrentTile = area.GetTile(StartPosition);
        area.SetTile(StartPosition, TileUser);

        if (!blocks.FirstOrDefault(p => p.Tile == CurrentTile)?.IsMotion ?? true)
        {
            throw new Exception("Попытка заспавнить игрока в блоке или блок под игроком не распознан");
        }
    }

    public void Motion()
    {
        IsKeyW = Input.GetKeyDown(KeyCode.W);
        IsKeyD = Input.GetKeyDown(KeyCode.D);
        IsKeyA = Input.GetKeyDown(KeyCode.A);
        IsKeyS = Input.GetKeyDown(KeyCode.S);

        if (IsKeyS || IsKeyW)
        {
            LineMotion(IsKeyW, IsKeyS, (p, c) => new Vector3Int(p.x, p.y + c * Step));
        }
        else if (IsKeyA || IsKeyD)
        {
            LineMotion(IsKeyD, IsKeyA, (p, c) => new Vector3Int(p.x + c * Step, p.y));
        }
    }

    private void LineMotion(bool key1, bool key2, Func<Vector3Int, int, Vector3Int> vectorFunc)
    {
        var c = key1 ? 1 : key2 ? -1 : 0;

        var lastPosition = CurrentPostition;
        var lastTile = CurrentTile;

        CurrentPostition = vectorFunc(CurrentPostition, c);
        CurrentTile = area.GetTile(CurrentPostition);

        if(!blocks.FirstOrDefault(p => p.Tile == CurrentTile)?.IsMotion ?? true)
        {
            CurrentPostition = lastPosition;
            CurrentTile = lastTile;
            return;
        }

        area.SetTile(lastPosition, lastTile);
        area.SetTile(CurrentPostition, TileUser);
    }


}
