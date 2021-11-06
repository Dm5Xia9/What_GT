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
}

[Serializable]
public class Chest
{
    public List<Item> Items;
    public TileBase Replacement;
    public void Open(Vector2Int position, Area area)
    {
        area.Tilemap.SetTile((Vector3Int)position, Replacement);

        var randomIndex = new System.Random().Next(0, Items.Count());
        area.LiesItemsObj.Add(Items[randomIndex], position);
    }
}
