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
    public Vector2Int CurrentPostition { get; set; }
    public TileBase CurrentTile { get; set; }

    public Vector2Int StartPosition { get; set; }
    public GameObject User { get; set; }
    public int Step { get; set; }

    private Tilemap tilemap;
    private List<Block> blocks;
    private Area area;

    private float currentXp;
    public bool IsDead { get; set; }
    public UserService(UserOptions userOptions, List<Block> blocks, Area area)
    {
        this.blocks = blocks;
        StartPosition = userOptions.StartPosition;
        User = userOptions.User;
        Step = userOptions.Step;
        this.area = area;
        tilemap = area.Tilemap;
        currentXp = userOptions.MaxXp;

        CurrentPostition = StartPosition;
        CurrentTile = tilemap.GetTile((Vector3Int)StartPosition);

        User = area.Inst(User, new Vector3(StartPosition.x + 0.5f, StartPosition.y + 0.5f), Quaternion.identity);

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
            LineMotion(IsKeyW, IsKeyS, (p, c) => new Vector2Int(p.x, p.y + c * Step));
        }
        else if (IsKeyA || IsKeyD)
        {
            LineMotion(IsKeyD, IsKeyA, (p, c) => new Vector2Int(p.x + c * Step, p.y));
        }
    }

    public void Hit(float hit)
    {
        currentXp -= hit;

        if (currentXp <= 0)
        {
            IsDead = true;
        }

        area.XpText.text = $"Здоровье: {currentXp}%";
    }

    private void LineMotion(bool key1, bool key2, Func<Vector2Int, int, Vector2Int> vectorFunc)
    {
        var c = key1 ? 1 : key2 ? -1 : 0;

        var lastPosition = CurrentPostition;
        var lastTile = CurrentTile;

        CurrentPostition = vectorFunc(CurrentPostition, c);
        CurrentTile = tilemap.GetTile((Vector3Int)CurrentPostition);

        if(!blocks.FirstOrDefault(p => p.Tile == CurrentTile)?.IsMotion ?? true)
        {
            CurrentPostition = lastPosition;
            CurrentTile = lastTile;
            return;
        }

        User.transform.position = new Vector3(CurrentPostition.x + 0.5f, CurrentPostition.y + 0.5f);


        var mob = area.MobsService.mobs.FirstOrDefault(p => p.CurrentPoint == CurrentPostition);
        if(mob != null)
        {
            Hit(mob.Damage);
        }
    }


}
