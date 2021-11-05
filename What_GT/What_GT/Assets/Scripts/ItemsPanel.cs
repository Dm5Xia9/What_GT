using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class ItemsPanel : MonoBehaviour
{
    public GameObject BasePanel;
    public int MaxElements;
    public List<Item> Items;
    public float LeftPadding;
    public float Ot;
    public float Do;
    public float Z;

    public int CurrentIndexItem = 0;

    public List<Item> ListUniqItems;

    void Start()
    {
        ListUniqItems = new List<Item>();
        var i = 1;
        var position = gameObject.transform.position;
        foreach (var item in Items)
        {
            var obj = item.gameObject;

            var start = Ot + ((item.IconSize.x + LeftPadding) * i);
            var go = Instantiate(item.gameObject, new Vector3(start, position.y, Z), Quaternion.identity, gameObject.transform);
            go.transform.localScale = item.IconSize;
            i++;

            if(i == 1)
            {
                var item2 = go.GetComponent<Item>();
                item2.IsSelect = true;
            }
        }

    }

    private void FixedUpdate()
    {
        ////float mw = Input.GetAxis("Mouse ScrollWheel");
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    Interlocked.Increment(ref CurrentIndexItem);

        //    if (CurrentIndexItem >= Items.Count)
        //        CurrentIndexItem = 0;

        //    print(CurrentIndexItem);
        //    Items.ForEach(p => p.IsSelect = false);
        //    Items[CurrentIndexItem].IsSelect = true;
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    Interlocked.Decrement(ref CurrentIndexItem);
        //    if (CurrentIndexItem < 0)
        //        CurrentIndexItem = 0;

        //    print(CurrentIndexItem);
        //    Items.ForEach(p => p.IsSelect = false);
        //    Items[CurrentIndexItem].IsSelect = true;
        //}

    }

    
}
