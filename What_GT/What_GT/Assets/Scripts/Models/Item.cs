using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public abstract class Item : MonoBehaviour
{
    public Vector3 IconSize;
    public bool IsSelect;
    public GameObject Border;
    public bool alreadySelect;
    public bool alreadyNoSelect;
    private GameObject activeBorder;
    public int DelayMilliseconds = 1000;
    public TimeSpan Delay => new TimeSpan(0, 0, 0, 0, DelayMilliseconds);
    public Vector3 SizeDropIcon;
    public Vector3 SizeMainIcon;

    public List<Shell> ShellObjs { get; set; }

    public int RandomStrength = 10;

    public float Strength { get; set; } = 100;
    private void Start()
    {
        ShellObjs = new List<Shell>();
    }

    public virtual void Action(Area area, KeyCode keyCode)
    {

    }
    //private void Start()
    //{
    //    alreadyNoSelect = true;
    //}

    protected void Broke(Area area)
    {
        var rd = new System.Random();

        var d = rd.Next(0, RandomStrength);

        if(d == 0)
        {
            Strength -= rd.Next(1, 100);

            if(Strength <= 0)
            {
                area.UserService.StrengthText.text = $"Оружие сломано";
                area.UserService.DelItem();

                Strength = 100;
            }
            else
            {
                area.UserService.StrengthText.text = $"Прочность: {Strength}";
            }
        }
    }

    private void Update()
    {
        //gameObject.transform.localScale = IconSize;

        //if (IsSelect && !alreadySelect)
        //{
        //    activeBorder = Instantiate(Border, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        //    activeBorder.transform.localPosition = new Vector3(0, 0, 0);
        //    activeBorder.transform.localScale = new Vector3(0.5f, 0.5f);
        //    alreadySelect = true;
        //    alreadyNoSelect = false;
        //}
        //else if(!IsSelect && !alreadyNoSelect)
        //{
        //    Destroy(activeBorder);
        //    alreadySelect = false;
        //    alreadyNoSelect = true;
        //}

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    IsSelect = !IsSelect;
        //}
    }
}
