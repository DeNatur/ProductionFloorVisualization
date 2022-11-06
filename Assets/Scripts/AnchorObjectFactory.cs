using UnityEngine;
using Zenject;

public class AnchorObjectFactory : IFactory<UnityEngine.Object, AnchorPresenter>
{
    readonly DiContainer _container;

    public AnchorObjectFactory(DiContainer container)
    {
        _container = container;
    }

    public AnchorPresenter Create(Object prefab)
    {
        return _container.InstantiatePrefabForComponent<AnchorPresenter>(prefab);
    }
}
