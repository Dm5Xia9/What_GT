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
    public Tilemap Tilemap => gameObject.GetComponentInParent<Tilemap>();
    public Text XpText;
    public Mobs MobsService { get; set; }
    void Start()
    {
        UserService = new UserService(UserOptions, Blocks.GetComponent<Blocks>().blocks, this);
        MobsService = Mobs.GetComponent<Mobs>();
        MobsService.Init(this);
    }

    void Update()
    {
        UserService.Motion();
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
