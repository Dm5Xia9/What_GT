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

        GameObject.transform.position = new Vector3(CurrentPoint.x + 0.5f, CurrentPoint.y + 0.5f);

        if(area.UserService.CurrentPostition == CurrentPoint)
        {
            area.UserService.Hit(Damage);
        }
    }
}

public enum Direction
{
    There,
    Back
}
