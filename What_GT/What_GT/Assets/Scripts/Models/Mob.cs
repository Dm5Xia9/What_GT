using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Mob
{
    public GameObject GameObject;
    public List<Vector2Int> Points;

    private int currentIndex;
    public Vector2Int CurrentPoint => Points[currentIndex];
    private Direction direction;
    public float Damage;
    public float Xp;

    public void Motion(Area area)
    {
        if(direction == Direction.There)
        {
            if(currentIndex + 1 == Points.Count)
            {
                direction = Direction.Back;

                currentIndex--;
            }
            else
            {
                currentIndex++;
            }
        }
        else if(direction == Direction.Back)
        {
            if (currentIndex == 0)
            {
                direction = Direction.There;

                currentIndex++;
            }
            else
            {
                currentIndex--;
            }
        }

        var destination = new Vector3(CurrentPoint.x + 0.5f, CurrentPoint.y + 0.5f);
        area.SetUpdateTask(() => GameObject.transform.position = Vector3.Lerp(GameObject.transform.position, destination, 0.1f),
            () => GameObject.transform.position == destination);

        if(area.UserService.CurrentPostition == CurrentPoint)
        {
            area.UserService.Hit(Damage);
        }

        if (area.UserService.Item is RangedWeapon wea)
        {
            var damages = wea.ShellObjs.Where(p => p != null && p.CurrentPosition == CurrentPoint);

            if (damages.Any())
            {
                Hit(damages.Sum(p => p.Damage));
            }

            foreach (var d in damages)
                d.Collision();
        }

        
    }

    public void Hit(float damage)
    {
        Xp -= damage;

        if (Xp <= 0)
            GameObject.Destroy(GameObject);

    }
}

public enum Direction
{
    There,
    Back
}
