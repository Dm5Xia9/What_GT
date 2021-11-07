using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Block
{
    public bool IsMotion = true;

    public TileBase Tile;

    public bool IsChest = false;
    public Chest Chest;

    public float GeneratorOt;
    public float GeneratorDo;

    public bool IsDoor;
}

[Serializable]
public class Chest
{
    public List<Item> Items;
    public TileBase Replacement;
    public Lies Open(Vector2Int position, Area area)
    {
        area.Tilemap.SetTile((Vector3Int)position, Replacement);

        var randomIndex = Area.rd.Next(0, Items.Count());
        return area.LiesItemsObj.Add(Items[randomIndex], position);
    }
}
