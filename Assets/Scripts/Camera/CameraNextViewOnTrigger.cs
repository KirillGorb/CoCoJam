using UnityEngine;
using Zenject;

namespace CodeScripts.Camera
{
    public class CameraNextViewOnTrigger : MonoBehaviour
    {
        [Inject] private CameraZoomAndMoveController _controllerActivate;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                _controllerActivate.SetNext(1);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                _controllerActivate.SetNext();
        }
    }
}