using UnityEngine;

namespace _Game.Scripts.RoundsSystem
{
    public class RoundsView : MonoBehaviour
    {
        [SerializeField] private AudioSource _penSound;
        [SerializeField] private GameObject[] _crests;

        public void EnableCrests(int count)
        {
            _penSound.Play();

            for (int i = 0; i < count; i++)
            {
                _crests[i].SetActive(true);
            }
        }

    }
}