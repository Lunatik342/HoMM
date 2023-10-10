using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetManagement
{
    public class AssetsLoadingService
    {
        public async UniTask<T> InstantiateAsync<T>(AssetReferenceGameObject assetReference, Vector3 position, Quaternion rotation, Transform parent) 
            where T: Component
        {
            var gameObject = await InstantiateAsync(assetReference, position, rotation, parent);
            return gameObject.GetComponent<T>();
        }

        public async UniTask<GameObject> InstantiateAsync(AssetReferenceGameObject assetReference, Vector3 position, Quaternion rotation, Transform parent)
        {
            var gameObject = await Addressables.InstantiateAsync(assetReference, position, rotation, parent).ToUniTask();
            gameObject.AddComponent<AssetReferenceReleaser>();
            return gameObject;
        }
    }
}