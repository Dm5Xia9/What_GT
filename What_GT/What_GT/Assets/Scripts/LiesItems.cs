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

    private List<Lies> MainItems = new List<Lies>();

    public void Init(Area area)
    {
        this.area = area;
        foreach (var item in Items)
        {
            //item.GameObject = area.Inst(item.Item.gameObject, new Vector3(item.Position.x + 0.5f, item.Position.y + 0.5f, -0.14f), Quaternion.identity);

            //item.GameObject.transform.localScale = item.Item.SizeDropIcon;

            MainItems.Add(item);
        }
    }

    public void Rerfash()
    {
        Items.Where(p => p != null && p.GameObject != null && p.Item != null && p.Item.gameObject != null && 
            !MainItems.Contains(p)).ToList().ForEach(p => Remove(p));

        Items.Clear();
        Items.AddRange(MainItems);
    }

    public Lies Add(Item item, Vector2Int position)
    {
        var lies = new Lies()
        {
            Item = item,
            Position = position,
        };

        lies.GameObject = area.Inst(lies.Item.gameObject, new Vector3(lies.Position.x + 0.5f, lies.Position.y + 0.5f, -0.14f), Quaternion.identity);
        lies.GameObject.transform.localScale = lies.Item.SizeDropIcon;
        Items.Add(lies);
        return lies;
    }

    public void Remove(Lies lies)
    {
        Destroy(lies.GameObject);
        Items.Remove(lies);
    }

    
}
