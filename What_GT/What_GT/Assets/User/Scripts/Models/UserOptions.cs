﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class UserOptions
{
    public Vector3Int StartPosition;
    public TileBase TileUser;
    public int Step;
}
