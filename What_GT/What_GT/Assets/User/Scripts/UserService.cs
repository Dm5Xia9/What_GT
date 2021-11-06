using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UserService
{
    public bool IsKeyW { get; set; }
    public bool IsKeyD { get; set; }
    public bool IsKeyA { get; set; }
    public bool IsKeyS { get; set; }
    public Vector2Int CurrentPostition { get; set; }
    public TileBase CurrentTile { get; set; }

    public Vector2Int StartPosition { get; set; }
    public GameObject User { get; set; }
    public int Step { get; set; }

    private Tilemap tilemap;
    private List<Block> blocks;
    private Area area;

    private float currentXp;
    public bool IsDead { get; set; }

    public Item Item { get; set; }
    private Text XpText;
    private Text RechargeText;
    private GameObject IconStrite;
    private Text Prompt;
    private GameObject ParentPrompt;
    private Sprite BaseIcon;
    private Text HilText;
    private int Hils;
    private Hilka hilka;

    private Text CoinsText;
    private int CoinsCount;

    public Text StrengthText;
    public UserService(UserOptions userOptions, List<Block> blocks, Area area)
    {
        this.blocks = blocks;
        StartPosition = userOptions.StartPosition;
        User = userOptions.User;
        Step = userOptions.Step;
        this.area = area;
        tilemap = area.Tilemap;
        currentXp = userOptions.MaxXp;
        Item = userOptions.Item;
        CurrentPostition = StartPosition;
        CurrentTile = tilemap.GetTile((Vector3Int)StartPosition);
        XpText = userOptions.XpText;
        RechargeText = userOptions.RechargeText;
        User = area.Inst(User, new Vector3(StartPosition.x + 0.5f, StartPosition.y + 0.5f), Quaternion.identity);
        IconStrite = userOptions.IconSprite;
        Prompt = userOptions.Prompt;
        ParentPrompt = Prompt.GetComponentInParent<Image>().gameObject;
        BaseIcon = userOptions.BaseIcon;
        HilText = userOptions.Hils;
        hilka = userOptions.Hilka;
        AddHilk(userOptions.BaseCountHils);

        CoinsText = userOptions.Moneys;
        AddCoin(userOptions.MoneysCount);

        StrengthText = userOptions.Strength;

        RefrashIcon();


        upDt = DateTime.Now;
        upDtMotion = DateTime.Now;
        ParentPrompt.SetActive(false);
    }

    private DateTime upDtMotion;
    public void Motion()
    {
        if (DateTime.Now - upDtMotion >= new TimeSpan(0, 0, 0, 0, 100))
        {
            IsKeyW = Input.GetKey(KeyCode.W);
            IsKeyD = Input.GetKey(KeyCode.D);
            IsKeyA = Input.GetKey(KeyCode.A);
            IsKeyS = Input.GetKey(KeyCode.S);

            if (IsKeyS || IsKeyW)
            {
                LineMotion(IsKeyW, IsKeyS, (p, c) => new Vector2Int(p.x, p.y + c * Step));
            }
            else if (IsKeyA || IsKeyD)
            {
                LineMotion(IsKeyD, IsKeyA, (p, c) => new Vector2Int(p.x + c * Step, p.y));
            }

            upDtMotion = DateTime.Now;
        }
        
    }

    private DateTime upDt;
    private bool shot;
    public void FireCheck()
    {
        if (Item == null)
            return;

        if (DateTime.Now - upDt >= Item.Delay)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Item.Action(area, KeyCode.DownArrow);
                shot = true;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                Item.Action(area, KeyCode.UpArrow);
                shot = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Item.Action(area, KeyCode.LeftArrow);
                shot = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Item.Action(area, KeyCode.RightArrow);
                shot = true;
            }
            if (shot)
            {
                upDt = DateTime.Now;
                shot = false;
            }
        }
    }

    private DateTime dtLock = DateTime.Now;
    private bool tk;
    public void TakingItems()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (DateTime.Now - dtLock < new TimeSpan(0, 0, 0, 0, 300))
                return;

            var item = area.LiesItemsObj.Items.FirstOrDefault(p => p.Position == CurrentPostition);

            if(item != null && item.Item is Hilka hil)
            {
                area.LiesItemsObj.Remove(item);
                takeType = TakeType.Give;
                RefrashPrompt(null);
                AddHilk(1);
                tk = true;
            }
            else if (item != null && item.Item is Coin coin)
            {
                area.LiesItemsObj.Remove(item);
                takeType = TakeType.Give;
                RefrashPrompt(null);
                AddCoin(1);
                tk = true;
            }
            else
            if (takeType == TakeType.My)
            {
                Item = item.Item;
                area.LiesItemsObj.Remove(item);
                takeType = TakeType.Give;
                RefrashIcon();
                RefrashPrompt(null);
                tk = true;
            }
            else if(takeType == TakeType.Exchange)
            {
                var lies = area.LiesItemsObj.Add(Item, CurrentPostition);
                Item = item.Item;
                area.LiesItemsObj.Remove(item);
                RefrashIcon();
                RefrashPrompt(lies);
                tk = true;
            }
            else if(takeType == TakeType.Give && item == null && Item != null)
            {
                var lies = area.LiesItemsObj.Add(Item, CurrentPostition);
                Item = null;
                takeType = TakeType.My;
                RefrashIcon();
                RefrashPrompt(lies);
                tk = true;
            }
            else if(takeType == TakeType.Chest)
            {
                var block = blocks.FirstOrDefault(p => p.Tile == CurrentTile);

                var lies = block.Chest.Open(CurrentPostition, area);

                if (Item == null)
                    takeType = TakeType.My;
                else
                    takeType = TakeType.Exchange;

                RefrashPrompt(lies);
                tk = true;
            }
            else if(takeType == TakeType.Door)
            {
                area.ReGeneration();
                tk = true;
            }

            if (tk)
            {
                dtLock = DateTime.Now;
                tk = false;
            }
        }
    }

    public void Consumables()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            UseHilk(hilka.Xp);
        }
    }

    private void AddCoin(int count)
    {
        CoinsCount += count;

        CoinsText.text = CoinsCount.ToString();
    }

    private void AddHilk(int count)
    {
        Hils += count;
        HilText.text = Hils.ToString();
    }

    public void UseHilk(float xp)
    {
        if (Hils <= 0)
            return;

        Hils--;
        HilText.text = Hils.ToString();

        currentXp += xp;

        if (currentXp >= 100)
            currentXp = 100;

        XpText.text = $"Здоровье: {currentXp}%";
    }

    public void DelItem()
    {
        Item = null;
        RefrashIcon();
    }

    private void RefrashIcon()
    {
        var render = IconStrite.GetComponent<SpriteRenderer>();
        if (Item == null)
        {
            render.sprite = BaseIcon;
            render.transform.localScale = new Vector3(1, 1, 1);
            return;
        }

        render.sprite = Item.gameObject.GetComponent<SpriteRenderer>().sprite;
        render.transform.localScale = Item.SizeMainIcon;

        StrengthText.text = $"Прочность: {Item.Strength}";
    }
    private void RefrashPrompt(Lies item)
    {
        var block = blocks.FirstOrDefault(p => p.Tile == CurrentTile);
        if (block.IsChest)
        {
            Prom("Нажмите \"E\" чтобы открыть", TakeType.Chest);
            return;
        }
        else if (block.IsDoor)
        {
            Prom("Телепортация...", TakeType.Door);
            return;
        }


        if (item != null)
        {

            if(item.Item is Hilka || item.Item is Coin)
            {
                Prom("Нажмите \"E\" чтобы взять", TakeType.My);
                return;
            }

            if (Item == null)
                Prom("Нажмите \"E\" чтобы взять", TakeType.My);
            else
                Prom("Нажмите \"E\" чтобы обменять", TakeType.Exchange);
        }
        else
        {
            NoProm();
        }
    }

    public void Recharge()
    {
        if(Item == null)
        {
            RechargeText.text = "Нету оружия";
            return;
        }

        var time = DateTime.Now - upDt;

        if(time >= Item.Delay)
        {
            RechargeText.text = "Оружие перезаряжено";
        }
        else
        {
            RechargeText.text = $"Перезаражается: {time.TotalMilliseconds / Item.Delay.TotalMilliseconds * 100}%";
        }

        area.Print($"R {Item.Delay.TotalMilliseconds}");
        area.Print($"S {time.TotalMilliseconds}");
    }

    public void Hit(float hit)
    {
        currentXp -= hit;

        if (currentXp <= 0)
        {
            IsDead = true;
        }

        XpText.text = $"Здоровье: {currentXp}%";
    }

    private void LineMotion(bool key1, bool key2, Func<Vector2Int, int, Vector2Int> vectorFunc)
    {
        var c = key1 ? 1 : key2 ? -1 : 0;

        var lastPosition = CurrentPostition;
        var lastTile = CurrentTile;

        CurrentPostition = vectorFunc(CurrentPostition, c);
        CurrentTile = tilemap.GetTile((Vector3Int)CurrentPostition);

        if(!blocks.FirstOrDefault(p => p.Tile == CurrentTile)?.IsMotion ?? true)
        {
            CurrentPostition = lastPosition;
            CurrentTile = lastTile;
            return;
        }

        var mob = area.MobsService.mobs.FirstOrDefault(p => p.CurrentPoint == CurrentPostition);
        if(mob != null)
        {
            Hit(mob.Damage);
        }

        if(Item is RangedWeapon wea)
        {
            var damages = wea.ShellObjs.Where(p => p != null && p.CurrentPosition == CurrentPostition);

            if (damages.Any())
            {
                Hit(damages.Sum(p => p.Damage));
            }

            foreach (var d in damages)
                d.Collision();
        }


        var destination = new Vector3(CurrentPostition.x + 0.5f, CurrentPostition.y + 0.5f);
        area.SetUpdateTask(() => User.transform.position = Vector3.Lerp(User.transform.position, destination, 0.1f),
            () => User.transform.position == destination);

        var item = area.LiesItemsObj.Items.FirstOrDefault(p => p.Position == CurrentPostition);
        RefrashPrompt(item);

    }
    TakeType takeType = TakeType.Give;
    private void NoProm()
    {
        takeType = TakeType.Give;
        ParentPrompt.SetActive(false);
    }
    private void Prom(string message, TakeType takeType)
    {
        this.takeType = takeType;
        ParentPrompt.SetActive(true);
        Prompt.text = message;
    }
}

public enum TakeType
{
    Give,
    My,
    Exchange,
    Chest,
    Door
}
