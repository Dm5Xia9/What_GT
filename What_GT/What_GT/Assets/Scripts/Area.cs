using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Area : MonoBehaviour
{
    //User
    public UserOptions UserOptions;
    public UserService UserService { get; set; }

    public GameObject Blocks;
    public GameObject Mobs;
    public GameObject LiesItems;

    public Tilemap Tilemap => gameObject.GetComponentInParent<Tilemap>();
    public Mobs MobsService { get; set; }
    public List<Block> BlockList { get; set; }
    public LiesItems LiesItemsObj { get; set; }
    public bool Generator;
    public GeneratorOptions generatorOptions;

    public int OtX;
    public int DoX;

    public int OtY;
    public int DoY;

    public Text killText;
    public Text houseText;

    public static Area Ar;
    void Start()
    {
        Ar = this;
        if (!Generator)
        {
            LiesItemsObj = LiesItems.GetComponent<LiesItems>();
            BlockList = Blocks.GetComponent<Blocks>().blocks;
            UserService = new UserService(UserOptions, BlockList, this);
            MobsService = Mobs.GetComponent<Mobs>();
            MobsService.Init(this);
            LiesItemsObj.Init(this);
        }
        else
        {
            LiesItemsObj = LiesItems.GetComponent<LiesItems>();
            LiesItemsObj.Init(this);

            BlockList = Blocks.GetComponent<Blocks>().blocks;
            var rd = new System.Random();
            UserService = new UserService(UserOptions, BlockList, this);
            MobsService = Mobs.GetComponent<Mobs>();
            MobsService.Init(this);
            MobsService.CreateMobs();
            generatorOptions.width = DoY - OtY;
            generatorOptions.height = DoX - OtX;
            generatorOptions.seed = rd.Next(1, 100000);
            var mapPoints = generatorOptions.Generate();

            var i = OtX;
                
            var countChest = 0;
            foreach(var line in mapPoints) 
            { 
                var j = OtY; 
                foreach (var colomn in line)
                {

                    var bl = BlockList.FirstOrDefault(p => p.GeneratorOt < colomn && p.GeneratorDo > colomn);

                    if (countChest <= 5)
                    {
                        if(rd.Next(0, 150) == 0)
                        {
                            bl = BlockList.FirstOrDefault(p => p.IsChest);
                            countChest++;
                        }
                    }


                    if (bl == null)
                        continue;

                    Tilemap.SetTile(new Vector3Int(i, j), bl.Tile);

                    if(rd.Next(0, 10) == 0)
                    {
                        var mo = MobsService.mobs[rd.Next(0, MobsService.mobs.Count())];
                        var obj = Instantiate(mo);
                        obj.Damage = rd.Next(0, 90);
                        obj.Xp = rd.Next(10, 150);
                        obj.Points = MobsService.GetRendomPath();

                        MobsService.mobs.Add(obj);
                    }

                    if (rd.Next(0, 300) == 0)
                    {
                        var mo = LiesItemsObj.Items[rd.Next(0, LiesItemsObj.Items.Count())].Item;
                        LiesItemsObj.Add(mo, new Vector2Int(i, j));
                    }

                    j++;
                }

                i++;
            }
        }
    }
    public int houseCount;
    public int killCount;
    public void ReGeneration()
    {
        if (MobsService.mobs.Where(p => p != null && p.gameObject != null && !MobsService.MainMobs.Contains(p)).Count() >= 8)
            return;

        var rd = new System.Random();
        MobsService.Clear();
        LiesItemsObj.Rerfash();
        generatorOptions.seed = rd.Next(1, 100000);
        var mapPoints = generatorOptions.Generate();

        var i = OtX;

        var countChest = 0;
        foreach (var line in mapPoints)
        {
            var j = OtY;
            foreach (var colomn in line)
            {

                var bl = BlockList.FirstOrDefault(p => p.GeneratorOt < colomn && p.GeneratorDo > colomn);

                if (countChest <= 5)
                {
                    if (rd.Next(0, 150) == 0)
                    {
                        bl = BlockList.FirstOrDefault(p => p.IsChest);
                        countChest++;
                    }
                }


                if (bl == null)
                    continue;

                Tilemap.SetTile(new Vector3Int(i, j), bl.Tile);

                if (rd.Next(0, 4) == 0)
                {
                    var mo = MobsService.mobs[rd.Next(0, MobsService.mobs.Count())];
                    var obj = Instantiate(mo);
                    obj.Damage = rd.Next(0, 90);
                    obj.Xp = rd.Next(10, 150);
                    obj.Points = MobsService.GetRendomPath();

                    MobsService.mobs.Add(obj);
                }

                if (rd.Next(0, 300) == 0)
                {
                    var mo = LiesItemsObj.Items[rd.Next(0, LiesItemsObj.Items.Count())].Item;
                    LiesItemsObj.Add(mo, new Vector2Int(i, j));
                }

                j++;
            }

            i++;
        }

        houseCount++;
        houseText.text = $"Пройдено комнат: {countChest}";

    }

    void FixedUpdate()
    {
        UserService.Motion();
        UserService.FireCheck();
        UserService.Recharge();
        UserService.TakingItems();
        UserService.Consumables();
    } 

    public void SetUpdateTask(Action action, Func<bool> func)
    {
        if(isEnd == true)
        {
            Thread.Sleep(100);
        }

        this.action = action;
        this.func = func;
        isEnd = false;
    }

    private Action action;
    private Func<bool> func;
    private bool isEnd = true;

    private void Update()
    {
        if (isEnd)
            return;

        try
        {
            action();

            isEnd = func();
        }
        catch
        {
            isEnd = true;
        }
    }

    public T Inst<T>(T original, Vector3 position, Quaternion rotation) where T : UnityEngine.Object
    {
        return Instantiate(original, position, rotation);
    }

    public void Print(object message)
    {
        //print(message);
    }

}
