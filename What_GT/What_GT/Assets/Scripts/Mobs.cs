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
    public List<List<Vector2Int>> Pole;
    public void Init(Area area)
    {
        this.area = area;
    }

    public List<Mob> MainMobs = new List<Mob>();
    public void CreateMobs()
    {
        foreach (var mob in mobs)
        {
            //Instantiate(mob.gameObject, new Vector3(mob.Points[0].x + 0.5f, mob.Points[0].y + 0.5f), Quaternion.identity);
            //print(mob.gameObject);

            MainMobs.Add(mob);
        }
    }

    private DateTime upDt;

    Dictionary<Mob, Vector3> mobVectors = new Dictionary<Mob, Vector3>();


    public void Clear()
    {
        mobs.Where(p => p != null && p.gameObject != null && !MainMobs.Contains(p)).ToList().ForEach(p => p.Hit(10000, isClear: true));
        mobs.Clear();
        mobs.AddRange(MainMobs);
    }

    void Update()
    {
        //if(DateTime.Now - upDt >= new TimeSpan(0, 0, 1))
        //{
        //    foreach(var mob in mobs.Where(p => p.GameObject != null).ToList())
        //    {
        //        mobVectors.Add(mob, mob.Motion(area));
        //    }
        //    upDt = DateTime.Now;
        //}

        //if (mobVectors.Any())
        //{
        //    var listRems = new List<Mob>();
        //    foreach (var m in mobVectors)
        //    {
        //        if (m.Key.GameObject.transform.position == m.Value)
        //            listRems.Add(m.Key);
        //        else
        //        {
        //            m.Key.GameObject.transform.position = Vector3.Lerp(m.Key.GameObject.transform.position, m.Value, 0.6f);
        //        }

        //    }

        //    foreach (var l in listRems)
        //        mobVectors.Remove(l);
        //}
    }

    public List<Vector2Int> GetRendomPath()
    {
        var rd = new System.Random();

        var rdx = rd.Next(area.OtX, area.DoX);
        var rdy = rd.Next(area.OtY, area.DoY);

        var nextPoint = new Vector2Int(rdx, rdy);

        var isEnd = false;

        var vectors = new List<Vector2Int>()
        {
            nextPoint,
        };

        while (!isEnd)
        {
            if (rd.Next(0, 300) == 0 || vectors.Count() > 500)
            {
                isEnd = true;
                break;
            }

            var r = rd.Next(0, 5);

            Vector2Int next;
            switch (r)
            {
                case 0:
                    next = new Vector2Int(nextPoint.x, nextPoint.y + 1);
                    break;
                case 1:
                    next = new Vector2Int(nextPoint.x + 1, nextPoint.y);
                    break;
                case 2:
                    next = new Vector2Int(nextPoint.x - 1, nextPoint.y);
                    break;
                case 3:
                    next = new Vector2Int(nextPoint.x, nextPoint.y - 1);
                    break;
                default:
                    next = new Vector2Int(nextPoint.x, nextPoint.y);
                    break;
            }

            if(nextPoint.x > area.OtX && nextPoint.x < area.DoX && nextPoint.y > area.OtY && nextPoint.y < area.DoY)
            {
                nextPoint = next;
                vectors.Add(nextPoint);
            }

        }


        if (vectors.Count() == 1)
            vectors.Add(vectors.First());
        return vectors;
    }
}
