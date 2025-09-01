using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.CardSystem;
using _Game.Scripts.CardSystem.EffectVariants;
using _Game.Scripts.CharactersSystem.Player;
using _Game.Scripts.Items;
using _Game.Scripts.Items.Variants;
using _Game.Scripts.RoundsSystem;
using _Game.Scripts.TableSystem;
using DG.Tweening;
using Game.ServiceLocator;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = Unity.Mathematics.Random;

namespace _Game.Scripts.CharactersSystem.Enemy
{
    public class EnemyController : CharacterController, IService, IDisposable
    {
        private Health _health;
        private EnemyView _enemyView;
        private HealthView _healthView;
        private Table _table;
        public override CharacterController Opponent => G.Get<PlayerController>();

        private List<Action> _actions = new List<Action>();
        private int _currentActionIndex;

        private List<Item> _items = new List<Item>();
        private Coroutine _actionCoroutine;
        private Coroutine _waitDoTurnCoroutine;
        
        public EnemyView EnemyView => _enemyView;
        public Health Health => _health;

        public void Initialize()
        {
            _healthView = Object.FindObjectOfType<HealthPanel>().EnemyHealthView;
            _table = Object.FindObjectOfType<Table>();
            _healthView.Init();
            _health = new Health();
            _health.OnHealthInited += _healthView.ActiveHealth;
            _health.OnHealthAdd += _healthView.AddHealth;
            _health.OnDamaged += _healthView.RemoveHealth;
            
            _health.Init(2);
            _enemyView = Object.FindObjectOfType<EnemyView>();
        }

        public override void AddItem(Item item)
        {
            _items.Add(item);
        }

        public override void LockNextTurn()
        {
            _enemyView.EnableLock();
            _canDoTurn = false;
        }

        private int _tryUnlockCount; 
        
        public override void TryUnlockTurn()
        {
            _tryUnlockCount++;

            if (_tryUnlockCount >= 2)
            {
                _tryUnlockCount = 0;
                _canDoTurn = true;
                _enemyView.DisableLock();
            }
        }

        public override void EnablePanickAnimation()
        {
            
        }

        public override void DisablePanickAnimation()
        {
            
        }

        private bool _wasShoot;
        
        public void DoTurn()
        {
            _wasShoot = false;
            _currentActionIndex = 0;
            _actions.Clear();

            _waitDoTurnCoroutine = G.Get<RoutineStarter>().StartCoroutine(WaitDoTurn());
        }

        public void StopWaitDoTurn()
        {
            if (_waitDoTurnCoroutine != null)
            {
                G.Get<RoutineStarter>().StopCoroutine(_waitDoTurnCoroutine);
                _waitDoTurnCoroutine = null;
            }
        }
        
        private IEnumerator WaitDoTurn()
        {
            yield return new WaitForSeconds(1.5f);
            
            if(!_health.HasMaxHealth())
            {
                TryGetItemAction(typeof(HealthBuff));
            }
            Debug.Log("Start turn");
            if (G.Get<DeckController>().OnlyAttackCards())
            {
                TryGetItemAction(typeof(DoubleDamage));
                _actions.Add(ShootIntoPlayer);
            }
            else if (G.Get<DeckController>().OnlyEmptyCards())
            {
                if (TryGetItemAction(typeof(YinYang)))
                {
                    TryGetItemAction(typeof(DoubleDamage));
                    _actions.Add(ShootIntoPlayer);
                }
                else
                {
                    _actions.Add(ShootIntoYourself);
                }
            }
            else
            {
                TryGetItemAction(typeof(Lock));
                TryGetItemAction(typeof(Hand));
                
                if (TryGetItemAction(typeof(Angel)))
                {
                    if(G.Get<DeckController>().GetTopCard().Effect is AttackEffect)
                    {
                        _actions.Add(ShootIntoPlayer);
                    }
                    else
                    {
                        _actions.Add(ShootIntoYourself);
                    }
                }
                else
                {
                    int random = UnityEngine.Random.Range(0, 5);
                    if(random < 3)
                    {
                        _actions.Add(ShootIntoPlayer);  
                    }
                    else
                    {
                        _actions.Add(ShootIntoYourself);
                    }
                }
            }

            DoAction();
        }

        private bool TryGetItemAction(Type itemType)
        {
            foreach (Item item in _items)
            {
                if (item.GetType() == itemType)
                {
                    item.Callback += DoAction;
                    _actions.Add(() =>
                    {
                        UseItem(item);
                    });
                    return true;
                }
            }
            
            return false;
        }
        
        private void ShootIntoYourself()
        {
            if (_wasShoot)
            {
                return;
            }

            Shoot(_table.EnemyTakeCardZone);
            _wasShoot = true;
        }

        private void ShootIntoPlayer()
        {
            if (_wasShoot)
            {
                return;
            }
            
            Shoot(_table.PlayerTakeCardZone);
            _wasShoot = true;
        }

        private void Shoot(TakeCardZone takeCardZone)
        {
            Card card = G.Get<DeckController>().GetTopCard();

            card.transform.DOMove(takeCardZone.transform.position, 0.25f)
                .OnComplete(() =>
                {
                    card.OnPlace(takeCardZone, this);
                });
        }

        public void StopAction()
        {
            if (_actionCoroutine != null)
            {
                G.Get<RoutineStarter>().StopCoroutine(_actionCoroutine);
            }
        }
        
        private void DoAction()
        {
            if (G.Get<DeckController>().IsEmpty())
            {
                return;
            }

            G.Get<RoutineStarter>().StartCoroutine(WaitDoAction());
        }

        private IEnumerator WaitDoAction()
        {
            yield return new WaitForSeconds(1f);
            _actions[_currentActionIndex]?.Invoke();
            _currentActionIndex++;
        }
        
        private void UseItem(Item item)
        {
            _items.Remove(item);
            item.Use();
        }

        public override void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            _enemyView.TakeDamage();
        }

        public override void AddHealth()
        {
            _health.AddOneHealth();   
        }

        public void Dispose()
        {
            _health.OnHealthAdd -= _healthView.AddHealth;
            _health.OnDamaged -= _healthView.RemoveHealth;
        }
    }
}