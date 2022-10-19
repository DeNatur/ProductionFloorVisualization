using UnityEngine;
using Zenject;

public class AnchorObjectFactory : IFactory<UnityEngine.Object, AnchorScript>
{
    readonly DiContainer _container;

    public AnchorObjectFactory(DiContainer container)
    {
        _container = container;
    }

    public AnchorScript Create(Object prefab)
    {
        return _container.InstantiatePrefabForComponent<AnchorScript>(prefab);
    }
}
