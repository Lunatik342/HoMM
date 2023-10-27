using UnityEngine;
using Zenject;

public class BilboardWorldUI : MonoBehaviour
{
    private Camera _camera;
    
    [Inject]
    void Construct(Camera mainCamera)
    {
        _camera = mainCamera;
    }

    //TODO 
    void LateUpdate()
    {
        transform.rotation = _camera.transform.rotation;
    }
}
