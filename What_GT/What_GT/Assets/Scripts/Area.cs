using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Area : MonoBehaviour
{
    //User
    public UserOptions UserOptions;
    public UserService UserService { get; set; }

    public GameObject Blocks;
    public GameObject Mobs;
    public Tilemap Tilemap => gameObject.GetComponentInParent<Tilemap>();

    void Start()
    {
        UserService = new UserService(UserOptions, Blocks.GetComponent<Blocks>().blocks, this);
        Mobs.GetComponent<Mobs>().Init(this);
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
