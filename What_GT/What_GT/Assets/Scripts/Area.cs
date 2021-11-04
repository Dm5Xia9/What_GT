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
    public Tilemap Tilemap => gameObject.GetComponentInParent<Tilemap>();

    void Start()
    {
        UserService = new UserService(UserOptions, Tilemap, Blocks.GetComponent<Blocks>().blocks);
    }

    void Update()
    {
        UserService.Motion();
    }
}
