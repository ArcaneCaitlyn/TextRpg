using System;
using System.Collections.Generic;

namespace SpartaVillageGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.Write("플레이어 이름을 입력하세요: ");
            string playerName = Console.ReadLine();

            Game game = new Game(playerName);
            game.Start();
        }
    }

    public class Game
    {
        Player player;
        Shop shop;

        public Game(string? playerName)
        {
            player = new Player(playerName, "전사");
            shop = new Shop(player);
        }

        public void Start()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ShowStatus();
                        break;
                    case "2":
                        ShowInventory();
                        break;
                    case "3":
                        shop.ShowShop();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다");
                        Pause();
                        break;
                }
            }
        }

        void ShowStatus()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
                player.PrintStatus();
                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input == "0") break;
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    Pause();
                }
            }
        }

        void ShowInventory()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
                player.PrintInventory();
                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input == "0") break;
                else if (input == "1")
                {
                    ManageEquip();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    Pause();
                }
            }
        }

        void ManageEquip()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
                player.PrintInventory(withNumber: true);
                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input == "0") break;
                int num;
                if (int.TryParse(input, out num))
                {
                    if (num >= 1 && num <= player.Inventory.Count)
                    {
                        player.ToggleEquip(num - 1);
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다");
                        Pause();
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    Pause();
                }
            }
        }

        void Pause()
        {
            Console.WriteLine("\n(아무 키나 누르세요)");
            Console.ReadKey();
        }
    }

    public class Player
    {
        public string Name;
        public string Job;
        public int Level;
        public int BaseAtk;
        public int BaseDef;
        public int HP;
        public int Gold;

        public List<Item> Inventory;

        public Player(string name, string job)
        {
            Name = name;
            Job = job;
            Level = 1;
            BaseAtk = 10;
            BaseDef = 5;
            HP = 100;
            Gold = 1500;
            Inventory = new List<Item>();

            // 기본 장비 예시
            Inventory.Add(new Item("무쇠갑옷", "방어력 +5", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 5, false, true));
            Inventory.Add(new Item("스파르타의 창", "공격력 +7", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 7, 0, false, true));
            Inventory.Add(new Item("낡은 검", "공격력 +2", "쉽게 볼 수 있는 낡은 검 입니다.", 2, 0, false, false));
        }

        public void PrintStatus()
        {
            int plusAtk = 0, plusDef = 0;
            foreach (var item in Inventory)
            {
                if (item.Equipped)
                {
                    plusAtk += item.Atk;
                    plusDef += item.Def;
                }
            }
            Console.WriteLine($"Lv. {Level:00}");
            Console.WriteLine($"{Name} ( {Job} )");
            Console.Write($"공격력 : {BaseAtk + plusAtk}");
            if (plusAtk > 0) Console.Write($" (+{plusAtk})");
            Console.WriteLine();
            Console.Write($"방어력 : {BaseDef + plusDef}");
            if (plusDef > 0) Console.Write($" (+{plusDef})");
            Console.WriteLine();
            Console.WriteLine($"체 력 : {HP}");
            Console.WriteLine($"Gold : {Gold} G");
        }

        public void PrintInventory(bool withNumber = false)
        {
            Console.WriteLine("[아이템 목록]");
            if (Inventory.Count == 0)
            {
                Console.WriteLine("보유 중인 아이템이 없습니다.");
                return;
            }
            int idx = 1;
            foreach (var item in Inventory)
            {
                string equipMark = item.Equipped ? "[E]" : "";
                string num = withNumber ? $"{idx} " : "";
                Console.WriteLine($"- {num}{equipMark}{item.Name} | {item.Option} | {item.Desc}");
                idx++;
            }
        }

        public void ToggleEquip(int index)
        {
            if (index < 0 || index >= Inventory.Count) return;
            Inventory[index].Equipped = !Inventory[index].Equipped;
        }

        public bool HasItem(string name)
        {
            foreach (var item in Inventory)
                if (item.Name == name)
                    return true;
            return false;
        }

        public void AddItem(Item item)
        {
            Inventory.Add(item);
        }
    }

    public class Item
    {
        public string Name;
        public string Option;
        public string Desc;
        public int Atk;
        public int Def;
        public bool Equipped;
        public bool IsDefault;

        public Item(string name, string option, string desc, int atk, int def, bool equipped = false, bool isDefault = false)
        {
            Name = name;
            Option = option;
            Desc = desc;
            Atk = atk;
            Def = def;
            Equipped = equipped;
            IsDefault = isDefault;
        }

        public Item Clone()
        {
            return new Item(Name, Option, Desc, Atk, Def, false, IsDefault);
        }
    }

    public class Shop
    {
        Player player;
        List<ShopItem> Items;

        public Shop(Player player)
        {
            this.player = player;
            Items = new List<ShopItem>
            {
                new ShopItem(new Item("수련자 갑옷", "방어력 +5", "수련에 도움을 주는 갑옷입니다.", 0, 5), 1000),
                new ShopItem(new Item("무쇠갑옷", "방어력 +9", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 9), 0, true), // 이미 구매 완료
                new ShopItem(new Item("스파르타의 갑옷", "방어력 +15", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 0, 15), 3500),
                new ShopItem(new Item("낡은 검", "공격력 +2", "쉽게 볼 수 있는 낡은 검 입니다.", 2, 0), 600),
                new ShopItem(new Item("청동 도끼", "공격력 +5", "어디선가 사용됐던거 같은 도끼입니다.", 5, 0), 1500),
                new ShopItem(new Item("스파르타의 창", "공격력 +7", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 7, 0), 0, true),
            };
        }

        public void ShowShop()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G\n");
                Console.WriteLine("[아이템 목록]");
                int idx = 1;
                foreach (var si in Items)
                {
                    string status = si.Purchased ? "구매완료" : $"{si.Price} G";
                    Console.WriteLine($"- {si.Item.Name,-10} | {si.Item.Option,-10} | {si.Item.Desc,-30} |  {status}");
                    idx++;
                }
                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input == "0") break;
                else if (input == "1")
                {
                    BuyItem();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    Pause();
                }
            }
        }

        void BuyItem()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G\n");
                Console.WriteLine("[아이템 목록]");
                int idx = 1;
                foreach (var si in Items)
                {
                    string status = si.Purchased ? "구매완료" : $"{si.Price} G";
                    Console.WriteLine($"- {idx} {si.Item.Name,-10} | {si.Item.Option,-10} | {si.Item.Desc,-30} |  {status}");
                    idx++;
                }
                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input == "0") break;
                int num;
                if (int.TryParse(input, out num))
                {
                    if (num >= 1 && num <= Items.Count)
                    {
                        var si = Items[num - 1];
                        if (si.Purchased)
                        {
                            Console.WriteLine("이미 구매한 아이템입니다");
                            Pause();
                        }
                        else if (player.Gold < si.Price)
                        {
                            Console.WriteLine("Gold 가 부족합니다.");
                            Pause();
                        }
                        else
                        {
                            si.Purchased = true;
                            player.Gold -= si.Price;
                            player.AddItem(si.Item.Clone());
                            Console.WriteLine("구매를 완료했습니다.");
                            Pause();
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다");
                        Pause();
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    Pause();
                }
            }
        }

        void Pause()
        {
            Console.WriteLine("\n(아무 키나 누르세요)");
            Console.ReadKey();
        }
    }

    public class ShopItem
    {
        public Item Item;
        public int Price;
        public bool Purchased;

        public ShopItem(Item item, int price, bool purchased = false)
        {
            Item = item;
            Price = price;
            Purchased = purchased;
        }
    }
}
