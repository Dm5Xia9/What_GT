using System;
using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        LiesItemsObj = LiesItems.GetComponent<LiesItems>();
        BlockList = Blocks.GetComponent<Blocks>().blocks;
        UserService = new UserService(UserOptions, BlockList, this);
        MobsService = Mobs.GetComponent<Mobs>();
        MobsService.Init(this);
        LiesItemsObj.Init(this);
    }

    void FixedUpdate()
    {
        UserService.Motion();
        UserService.FireCheck();
        UserService.Recharge();
        UserService.TakingItems();
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
        print(message);
    }

}
