using UnityEngine;
using Zenject;

namespace UI.World
{
    public class WorldUILookingAtCamera : MonoBehaviour
    {
        private Camera _camera;
    
        [Inject]
        void Construct(Camera mainCamera)
        {
            _camera = mainCamera;
        }

        //TODO only change rotation on camera position (rotation) change
        void LateUpdate()
        {
            transform.rotation = _camera.transform.rotation;
        }
    }
}
