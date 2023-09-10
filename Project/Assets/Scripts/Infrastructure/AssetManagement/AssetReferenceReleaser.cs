using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetManagement
{
    public class AssetReferenceReleaser: MonoBehaviour
    {
        private void OnDestroy()
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}