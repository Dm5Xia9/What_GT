using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LiesItems : MonoBehaviour
{
    public List<Lies> Items;

    private Area area;
    public void Init(Area area)
    {
        this.area = area;
        foreach (var item in Items)
        {
            item.GameObject = area.Inst(item.Item.gameObject, new Vector3(item.Position.x + 0.5f, item.Position.y + 0.5f), Quaternion.identity);

            item.GameObject.transform.localScale = new Vector3(1, 1);
        }
    }

    public Lies Add(Item item, Vector2Int position)
    {
        var lies = new Lies()
        {
            Item = item,
            Position = position
        };

        lies.GameObject = area.Inst(lies.Item.gameObject, new Vector3(lies.Position.x + 0.5f, lies.Position.y + 0.5f), Quaternion.identity);
        Items.Add(lies);
        return lies;
    }

    public void Remove(Lies lies)
    {
        Destroy(lies.GameObject);
        Items.Remove(lies);
    }

    
}
