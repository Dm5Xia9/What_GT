using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GrenadeItem : Item
{
    public GameObject Grenade;
    public override void Action(Area area, KeyCode keyCode)
    {
        var qrenade = Instantiate(Grenade);
        var qrenadeObj = qrenade.GetComponent<Grenade>();
        if (qrenadeObj == null)
            print("null");
        qrenadeObj.Init(area, keyCode);

        ShellObjs.Add(qrenadeObj);
    }
}
