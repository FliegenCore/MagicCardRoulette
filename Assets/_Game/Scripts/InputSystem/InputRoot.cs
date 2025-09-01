using _Game.Scripts.PlayerInput.Variants;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.PlayerInput
{
    public class InputRoot : MonoBehaviour, IService
    {
        private DragAndDropCardInteractor _dragAndDropCardInteractor;
        private DescriptionInteractor _descriptionInteractor;
        private ClickInteractor _clickInteractor;

        private Raycaster _raycaster;
        private Interactor _currentInteractor;
        private Camera _camera;
        private bool _enabled;
        private bool _hardEnabled;
        
        public void Initialize()
        {
            _camera = Camera.main;
            _enabled = false;
            _hardEnabled = true;
            PosInit();
        }

        private void PosInit()
        {
            _raycaster = new Raycaster(_camera);
            _dragAndDropCardInteractor = new DragAndDropCardInteractor(_raycaster);
            _descriptionInteractor = new DescriptionInteractor(_raycaster);
            _clickInteractor = new ClickInteractor(_raycaster);
        }

        private void Update()
        {
            _descriptionInteractor.SelfUpdate();
            if(!_hardEnabled)
                return;
            if (!_enabled)
                return;
            
            if(_currentInteractor != null)
                _currentInteractor.SelfUpdate();

            ChooseInteractor();
        }

        public void HardDisable()
        {
            _hardEnabled = false;
        }
        
        public void Disable()
        {
            Debug.Log("Disable");
            _enabled = false;
        }

        public void Enable()
        {
            _enabled = true;
        }

        private void ChooseInteractor()
        {
            if (_clickInteractor.TryInteract())
                _currentInteractor = _clickInteractor;
            else if (_dragAndDropCardInteractor.TryInteract())
                _currentInteractor = _dragAndDropCardInteractor;
        }
    }
}