using System;
using System.Collections;
using _Game.Scripts.CharactersSystem;
using _Game.Scripts.CharactersSystem.Enemy;
using _Game.Scripts.CharactersSystem.Player;
using _Game.Scripts.TableSystem;
using Game.ServiceLocator;
using UnityEngine;
using CharacterController = _Game.Scripts.CharactersSystem.CharacterController;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Items
{
    public enum ItemsNames
    {
        Angel,
        Health,
        YinYang,
        DoubleDamage,
        Hand,
        Lock
    }

    public class ItemsController : IService
    {
        private Table _table;

        private bool _enable;
        
        public void Initialize()
        {
            _table = Object.FindObjectOfType<Table>();
        }

        public void EnableItems()
        {
            _enable = true;
        }

        public IEnumerator GiveItems(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return null;
                GiveItemToPlayer();
                yield return null;
                GiveItemToEnemy();
            }
        }

        public void GiveItemToPlayer()
        {
            GiveItem(G.Get<PlayerController>(), G.Get<EnemyController>(), _table.PlayerItemsZone);
        }

        public void GiveItemToEnemy()
        {
            GiveItem(G.Get<EnemyController>(), G.Get<PlayerController>(), _table.EnemyItemsZone);
        }

        private void GiveItem(CharacterController owner, CharacterController opponent, ItemZone[] zones)
        {
            bool hasEmptyZone = false;

            foreach (var zone in zones)
            {
                if (zone.CanSetItem())
                {
                    hasEmptyZone = true;
                    break;
                }
            }

            if (!hasEmptyZone)
            {
                return;
            }
            
            Item item = CreateItem(GetRandomItemName());
            
            item.SetOwnerAndOpponent(owner, opponent);
            
            owner.AddItem(item);
            
            foreach (var itemZone in zones)
            {
                if (itemZone.CanSetItem())
                {
                    itemZone.SetItem(item);
                    item.transform.position = itemZone.transform.position;
                    break;
                }
            }
        }

        private string GetRandomItemName()
        {
            int itemsCount = Enum.GetNames(typeof(ItemsNames)).Length;
            int itemIndex = Random.Range(0, itemsCount);
            string itemName = Enum.GetName(typeof(ItemsNames), itemIndex);

            return itemName;
        }

        private Item CreateItem(string itemName)
        {
            var asset = Resources.Load<Item>("Prefabs/Items/" + itemName);

            Item item = Object.Instantiate(asset);
            
            return item;
        }
    }
}