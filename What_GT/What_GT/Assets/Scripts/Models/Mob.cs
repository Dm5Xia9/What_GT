using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public List<Vector2Int> Points;

    private int currentIndex;
    public Vector2Int CurrentPoint => Points[currentIndex];
    private Direction direction;
    public float Damage;
    public float Xp;
    Area area;
    private void Start()
    {
        area = Area.Ar;

        upDt = DateTime.Now;

        
    }


    private DateTime upDt;
    private void FixedUpdate()
    {
        if (area == null)
            return;

        if (DateTime.Now - upDt >= new TimeSpan(0, 0, 0, 0, 500))
        {
            Motion(area);
            upDt = DateTime.Now;
        }
    }

    Vector3? destination = null;
    public void Update()
    {
        if (!destination.HasValue || destination == gameObject.transform.position)
            return;


        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, destination.Value, 0.1f);
    }

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
        //area.SetUpdateTask(() => GameObject.transform.position = Vector3.Lerp(GameObject.transform.position, destination, 0.1f),
        //    () => GameObject.transform.position == destination);

        if(area.Tilemap.GetTile((Vector3Int)CurrentPoint) == null)
        {
            Hit(1000);
            return;
        }

        if (area.UserService.CurrentPostition == CurrentPoint)
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

        destination = new Vector3(CurrentPoint.x + 0.5f, CurrentPoint.y + 0.5f, -0.1f);
    }

    public void Hit(float damage, bool isClear = false)
    {
        Xp -= damage;
        area.MobsService.KillAudio.Play();
        if (Xp <= 0 && gameObject != null)
        {
            Destroy(gameObject);
            if (!isClear)
            {
                area.killCount++;
                area.killText.text = $"Убито: {area.killCount}";
            }

        }

    }
}

public enum Direction
{
    There,
    Back
}
