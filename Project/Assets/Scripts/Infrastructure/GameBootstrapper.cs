using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoBehaviour
{
    private SceneLoader _sceneLoader;
    
    [Inject]
    public void Construct(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }
    
    public void Start()
    {
        _sceneLoader.LoadMainMenuScene().Forget(Debug.LogError);
    }
}
