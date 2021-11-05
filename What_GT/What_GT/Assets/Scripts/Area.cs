using System.Collections;
using System.Collections.Generic;
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

    public T Inst<T>(T original, Vector3 position, Quaternion rotation) where T : Object
    {
        return Instantiate(original, position, rotation);
    }

    public void Print(object message)
    {
        print(message);
    }

}
