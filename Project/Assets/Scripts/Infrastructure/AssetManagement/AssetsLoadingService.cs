using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetManagement
{
    public class AssetsLoadingService
    {
        public async Task<GameObject> Instantiate(AssetReferenceGameObject assetReference, Vector3 position, Quaternion rotation, Transform parent)
        {
            var gameObject = await Addressables.InstantiateAsync(assetReference, position, rotation, parent).Task;
            gameObject.AddComponent<AssetReferenceReleaser>();
            return gameObject;
        }
    }
}