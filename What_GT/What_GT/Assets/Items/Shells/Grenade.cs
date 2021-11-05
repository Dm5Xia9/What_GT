using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grenade : Shell
{
    public override void Collision()
    {
        BoundsInt bounds = area.Tilemap.cellBounds;
        List<TileBase> tiles = new List<TileBase>();

        //להאא
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
        }

        area.MobsService.mobs.Where(p => p.GameObject != null).Where(p => vectors.Contains(p.CurrentPoint)).ToList().
            ForEach(p => p.Hit(Damage));

        if (vectors.Contains(area.UserService.CurrentPostition))
        {
            area.UserService.Hit(Damage);
        }

        base.Collision();
    }
}
