using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RangedWeapon : Item
{
    public GameObject Shell;
    public List<Shell> ShellObjs { get; set; }

    private void Start()
    {
        ShellObjs = new List<Shell>();
    }

    public override void Action(Area area, KeyCode keyCode)
    {
        var shell = Instantiate(Shell);
        var shellObj = shell.GetComponent<Shell>();
        if (shellObj == null)
            print("null");
        shellObj.Init(area, keyCode);

        ShellObjs.Add(shellObj);
    }
}
