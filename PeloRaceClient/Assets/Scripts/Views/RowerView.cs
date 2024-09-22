using System.Collections;
using Services;
using UnityEngine;

namespace GameLogic
{
    public class RowerView : MonoBehaviour
    {
        private RowerViewService _service;
        private Vector3 _startPosition;

        [SerializeField] private GameRunner.RowerId _id;
        
        private IEnumerator Start()
        {
            // Wait for RowerViewService to instantiate to avoid race conditions
            while (RowerViewService.Instance == null)
            {
                yield return null;
            }
            _service = RowerViewService.Instance;
            _startPosition = transform.position;
            _service.Join(_id, _startPosition);
            _service.StateUpdated += ServiceStateUpdated;
            ServiceStateUpdated();
        }

        private void ServiceStateUpdated()
        {
            transform.position = _service.GetPosition(_id);
        }

        private void OnDestroy()
        {
            if (_service == null) return;
            _service.StateUpdated -= ServiceStateUpdated;
            _service.Leave(_id);
            _service = null;
        }
    }
}