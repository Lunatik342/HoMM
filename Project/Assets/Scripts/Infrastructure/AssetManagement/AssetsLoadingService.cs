using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetManagement
{
    public class AssetsLoadingService
    {
        public async Task<T> InstantiateAsync<T>(AssetReferenceGameObject assetReference, Vector3 position, Quaternion rotation, Transform parent) 
            where T: Component
        {
            var gameObject = await Addressables.InstantiateAsync(assetReference, position, rotation, parent).ToUniTask();
            gameObject.AddComponent<AssetReferenceReleaser>();
            return gameObject.GetComponent<T>();
        }
    }
}