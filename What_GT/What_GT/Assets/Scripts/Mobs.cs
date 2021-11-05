using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mobs : MonoBehaviour
{
    public List<Mob> mobs;

    private Area area;
    public void Init(Area area)
    {
        this.area = area;
        foreach(var mob in mobs)
        {
            mob.GameObject = area.Inst(mob.GameObject, new Vector3(mob.Points[0].x + 0.5f, mob.Points[0].y + 0.5f), Quaternion.identity);
        }
    }

    private DateTime upDt;
    void Update()
    {
        if(DateTime.Now - upDt >= new TimeSpan(0, 0, 1))
        {
            mobs.Where(p => p.GameObject != null).ToList().ForEach(p => p.Motion(area));
            upDt = DateTime.Now;
        }

    }
}
