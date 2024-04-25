using System;
using System.Collections.Generic;

namespace ConsoleApp9
{
    class Program
    {
        interface IAction
        {
            void Show();
        }

        class Item
        {
            public string Name { get; set; }
            public string Ability { get; set; }
            public int Attack { get; set; }
            public int Defense { get; set; }
            public string Description { get; set; }
            public bool Equipped { get; set; }
            public bool Purchased { get; set; }
            public int Price { get; set; }

            public Item(string name, string ability, int attack, int defense, string description, int price)
            {
                Name = name;
                Ability = ability;
                Attack = attack;
                Defense = defense;
                Description = description;
                Price = price;
                Equipped = false;
                Purchased = false;
            }
        }
        class PlayerStatus : IAction
        {
            private Player player;
            public PlayerStatus(Player player)
            {
                this.player = player;
            }
            public void Show()
            {

                Console.WriteLine("상태보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine($"Lv. {player.Level:D2}");
                Console.WriteLine($"{player.Name} ( {player.Job} )");
                Console.WriteLine($"공격력 : {player.Attack}");
                Console.WriteLine($"방어력 : {player.Defense}");
                Console.WriteLine($"체 력 : {player.Health}");
                Console.WriteLine($"Gold : {player.Gold} G");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해주세요: ");
                string input = Console.ReadLine();
                if (input == "0")
                    return;
                else
                    Console.WriteLine("잘못된 입력입니다.");
            }
        }

        class Inventory : IAction
        {
            private Player player;

            public Inventory(Player player)
            {
                this.player = player;
            }

            public void Show()
            {

                Console.WriteLine("인벤토리");
                Console.WriteLine("인벤토리를 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                List<Item> items = player.Inventory; // Player 클래스의 Inventory 속성 사용

                if (items.Count == 0)
                {
                    Console.WriteLine("아이템이 없습니다.");
                }
                else
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        string equipped = (items[i].Equipped) ? "" : "[E]";
                        Console.WriteLine($"- {equipped}{items[i].Name} | {items[i].Ability} | {items[i].Description}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해주세요: ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        return;
                    case "1":
                        ManageEquipment();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }

            private void ManageEquipment()
            {
                EquipmentManager equipmentManager = new EquipmentManager(player);
                equipmentManager.Show();
            }
        }

        class EquipmentManager : IAction
        {
            private Player player;

            public EquipmentManager(Player player)
            {
                this.player = player;
            }

            public void Show()
            {

                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                List<Item> items = player.Inventory; // Player 클래스의 Inventory 속성 사용

                if (items.Count == 0)
                {
                    Console.WriteLine("아이템이 없습니다.");
                }
                else
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        string equipped = (items[i].Equipped) ? "" : "[E]";
                        Console.WriteLine($"- {i + 1} {equipped}{items[i].Name} | {items[i].Ability} | {items[i].Description}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.Write("장착 또는 해제할 아이템 번호를 입력하세요 (0: 나가기): ");
                string input = Console.ReadLine();
                if (input == "0")
                    return;

                int itemIndex;
                if (int.TryParse(input, out itemIndex) && itemIndex > 0 && itemIndex <= items.Count)
                {
                    Item selected = items[itemIndex - 1];
                    if (!selected.Equipped)
                    {
                        // 이미 장착된 아이템인지 확인
                        bool alreadyEquipped = false;
                        foreach (Item item in items)
                        {
                            if (item.Equipped && item != selected)
                            {
                                alreadyEquipped = true;
                                break;
                            }
                        }

                        if (!alreadyEquipped)
                        {
                            selected.Equipped = true;
                            Console.WriteLine($"아이템 '{selected.Name}'을(를) 장착했습니다.");
                        }
                        else
                        {
                            Console.WriteLine("이미 다른 아이템이 장착되어 있습니다.");
                        }
                    }
                    else
                    {
                        selected.Equipped = false;
                        Console.WriteLine($"아이템 '{selected.Name}'의 장착을 해제했습니다.");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }




        class Shop : IAction
        {
            private Player player;
            private ItemManager itemManager;

            public Shop(Player player, ItemManager itemManager)
            {
                this.player = player;
                this.itemManager = itemManager;
            }

            public void Show()
            {
               
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < itemManager.Items.Count; i++)
                {
                    if (itemManager.Items[i].Attack == 0)
                    {
                        string purchasedStatus = (itemManager.Items[i].Purchased) ? "구매완료" : $"{itemManager.Items[i].Price} G";
                        Console.WriteLine($"- {itemManager.Items[i].Name} | {itemManager.Items[i].Ability} {itemManager.Items[i].Defense} | {itemManager.Items[i].Description} | {purchasedStatus}");
                    }
                    else if(itemManager.Items[i].Defense == 0)
                    {
                        string purchasedStatus = (itemManager.Items[i].Purchased) ? "구매완료" : $"{itemManager.Items[i].Price} G";
                        Console.WriteLine($"- {itemManager.Items[i].Name} | {itemManager.Items[i].Ability} {itemManager.Items[i].Attack} | {itemManager.Items[i].Description} | {purchasedStatus}");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해주세요: ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        return;
                    case "1":
                        var purchase = new ItemPurchase(this, player, itemManager);
                        purchase.Show();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }

        class ItemPurchase : IAction
        {
            private Shop shop;
            private Player player;
            private ItemManager itemManager;

            public ItemPurchase(Shop shop, Player player, ItemManager itemManager)
            {
                this.shop = shop;
                this.player = player;
                this.itemManager = itemManager;
            }

            public void Show()
            {
                
                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < itemManager.Items.Count; i++)
                {
                    if (itemManager.Items[i].Attack == 0)
                    {
                        string purchasedStatus = (itemManager.Items[i].Purchased) ? "구매완료" : $"{itemManager.Items[i].Price} G";
                        Console.WriteLine($"- {itemManager.Items[i].Name} | {itemManager.Items[i].Ability} {itemManager.Items[i].Defense} | {itemManager.Items[i].Description} | {purchasedStatus}");
                    }
                    else if (itemManager.Items[i].Defense == 0)
                    {
                        string purchasedStatus = (itemManager.Items[i].Purchased) ? "구매완료" : $"{itemManager.Items[i].Price} G";
                        Console.WriteLine($"- {itemManager.Items[i].Name} | {itemManager.Items[i].Ability} {itemManager.Items[i].Attack} | {itemManager.Items[i].Description} | {purchasedStatus}");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해주세요: ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        return;
                    default:
                        int itemIndex;
                        if (int.TryParse(input, out itemIndex) && itemIndex > 0 && itemIndex <= itemManager.Items.Count)
                        {
                            BuyItem(itemIndex);
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                        break;
                }
            }

            private void BuyItem(int index)
            {
                Item selected = itemManager.Items[index - 1];
                if (selected.Price <= player.Gold && !selected.Purchased)
                {
                    player.Gold -= selected.Price;
                    selected.Purchased = true;
                    player.Inventory.Add(selected); // 인벤토리에 아이템 추가
                    Console.WriteLine($"'{selected.Name}'을(를) 구매했습니다.");
                }
                else if (selected.Purchased)
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else
                {
                    Console.WriteLine("보유한 골드가 부족하거나 구매할 수 없는 아이템입니다.");
                }
            }
        }


        class Player
        {
            public int Level { get; set; }
            public string Name { get; set; }
            public string Job { get; set; }
            public int Attack { get; set; }
            public int Defense { get; set; }
            public int Health { get; set; }
            public int Gold { get; set; }
            public List<Item> Inventory { get; set; }

            public Player(string name, string job, int attack, int defense, int health, int gold)
            {
                Level = 1;
                Name = name;
                Job = job;
                Attack = attack;
                Defense = defense;
                Health = health;
                Gold = gold;
                Inventory = new List<Item>();
            }

            public void EquipItem(Item item)
            {
                item.Equipped = true;
                Console.WriteLine($"아이템 '{item.Name}'을(를) 장착했습니다.");
            }
        }


        class ItemManager
        {
            public List<Item> Items { get; private set; }

            public ItemManager()
            {
                Items = new List<Item>
            {
                new Item("무쇠갑옷", "방어력 +", 0, 9, " | 무쇠로 만들어져 튼튼한 갑옷입니다.", 1000),
                new Item("스파르타의 창", "공격력 +", 7, 0, " | 스파르타의 전사들이 사용했다는 전설의 창입니다.", 2000),
                new Item("낡은 검", "공격력 +",2, 0, " | 쉽게 볼 수 있는 낡은 검 입니다.", 600),
                new Item("수련자 갑옷", "방어력 +", 0, 5, " | 수련에 도움을 주는 갑옷입니다.", 1000),
                new Item("청동 도끼", "공격력 +", 5, 0, " | 어디선가 사용됐던거 같은 도끼입니다.", 1500),
                new Item("스파르타의 갑옷", "방어력 +", 0, 15,  " | 스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500)
            };
            }
        }


        static void Main(string[] args)
        {
            Player player = new Player("Chad", "전사", 10, 5, 100, 1500);
            ItemManager itemManager = new ItemManager();
            while (true)
            {

                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해주세요: ");

                string input = Console.ReadLine();

                IAction action;

                switch (input)
                {
                    case "1":
                        action = new PlayerStatus(player);
                        break;
                    case "2":
                        action = new Inventory(player);
                        break;
                    case "3":
                        action = new Shop(player, itemManager);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        return;
                }

                action.Show();
            }
        }
    }
}
