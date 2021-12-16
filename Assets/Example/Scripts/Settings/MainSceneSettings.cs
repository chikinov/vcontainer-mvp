using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Example
{
    [Serializable]
    public class MainSceneSettings
    {
        [field: SerializeField]
        public AssetReferenceGameObject RootViewRef { get; private set; }

        [field: SerializeField]
        public AssetReferenceGameObject MainViewRef { get; private set; }
    }
}
