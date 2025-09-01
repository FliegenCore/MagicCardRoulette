using System.Collections;
using DragonBones;
using UnityEngine;

namespace _Game.Scripts.CharactersSystem
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private UnityArmatureComponent[] _health;

        private int _healthCount = 0;

        public void Init()
        {
            _health = transform.GetComponentsInChildren<UnityArmatureComponent>();

            foreach (var health in _health)
            {
                health.animation.Play("idle");
                health.gameObject.SetActive(false);
            }
        }

        public void ActiveHealth(int health)
        {
            for (int i = 0; i < health; i++)
            {
                _health[i].gameObject.SetActive(true);
                var anim = _health[i].animation.Play("plus");
                StartCoroutine(StopAnimation(anim._duration, _health[i]));
            }
            
            _healthCount = health;
        }

        public void AddHealth(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _health[_healthCount + i].gameObject.SetActive(true);
                var anim = _health[_healthCount + i].animation.Play("plus");
                StartCoroutine(StopAnimation(anim._duration, _health[_healthCount + i]));
            }
            _healthCount += count;
        }
        

        public void RemoveHealth(int count)
        {
            for (int i = count; i > 0; i--)
            {
                var anim = _health[_healthCount - i].animation.Play("minus");
                StartCoroutine(DisableHealth(anim._duration, _health[_healthCount - i]));
            }

            _healthCount -= count;
        }

        private IEnumerator StopAnimation(float duration, UnityArmatureComponent component)
        {
            yield return new WaitForSeconds(duration);
            component.animation.GotoAndPlayByFrame("idle", 1);
        }
        
        private IEnumerator DisableHealth(float duration, UnityArmatureComponent component)
        {
            yield return new WaitForSeconds(duration);
            component.gameObject.SetActive(false);
        }

    }
}