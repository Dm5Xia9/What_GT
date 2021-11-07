using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grenade : Shell
{
    public int CountBlock = 6;
    public override void Collision()
    {
        BoundsInt bounds = area.Tilemap.cellBounds;
        List<TileBase> tiles = new List<TileBase>();

        //мдаа
        List<Vector2Int> vectors = new List<Vector2Int>
        {
            CurrentPosition,
            new Vector2Int(CurrentPosition.x + 1, CurrentPosition.y),
            new Vector2Int(CurrentPosition.x - 1, CurrentPosition.y),
            new Vector2Int(CurrentPosition.x, CurrentPosition.y + 1),
            new Vector2Int(CurrentPosition.x, CurrentPosition.y - 1),
        };

        foreach(var vector in vectors)
        {
            area.Tilemap.SetTile((Vector3Int)vector, null);
            area.map.SetTile((Vector3Int)vector, null);
        }

        var e1 = area.MobsService.mobs.Where(p => p != null && p.gameObject != null);

        var e2 = e1.Where(p => vectors.Contains(p.CurrentPoint)).ToList();

        e2.ForEach(p => p.Hit(Damage));

        if (vectors.Contains(area.UserService.CurrentPostition))
        {
            area.UserService.Hit(Damage, "Последний раз умер от своего же взрыва");
        }

        base.Collision();
    }

    public override bool FixedEnd()
    {
        if (CountStep >= CountBlock)
            return true;
        else
            return false;
    }
}
