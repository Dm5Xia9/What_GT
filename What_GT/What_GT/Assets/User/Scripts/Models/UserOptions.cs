using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[Serializable]
public class UserOptions
{
    public Vector2Int StartPosition;
    public GameObject User;
    public int Step;
    public float MaxXp;
    public Item Item;
    public Text XpText;
    public Text RechargeText;
    public GameObject IconSprite;
    public Sprite BaseIcon;
    public Text Prompt;
    public Text Hils;
    public int BaseCountHils;
    public Hilka Hilka;
    public Text Moneys;
    public int MoneysCount;
    public Text Message;
    public Text Strength;
}
