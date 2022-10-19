using Microsoft.Azure.SpatialAnchors.Unity;
using Zenject;

public class AppInstaller : MonoInstaller
{

    public AzureSessionCoordinator sessionCoordinator;
    public AddAnchorUseCase addAnchorUseCase;
    public SpatialAnchorManager cloudManager;
    public ObjectsCreator objectsCreator;
    public SceneAwarnessValidator sceneAwarnessValidator;

    public override void InstallBindings()
    {
        Container.Bind<AnchorsRepository>()
            .To<AzureAnchorsReporitory>()
            .AsSingle();

        Container.Bind<AddAnchorUseCase>()
            .AsSingle();

        Container.Bind<RemoveAnchorUseCase>()
            .AsSingle();

        Container.Bind<AzureSessionCoordinator>()
            .FromInstance(sessionCoordinator)
            .NonLazy();

        Container.Bind<SpatialAnchorManager>()
            .FromInstance(cloudManager)
            .AsSingle();

        Container.Bind<ObjectsCreator>()
            .FromInstance(objectsCreator)
            .AsSingle();

        Container.Bind<SceneAwarnessValidator>()
            .FromInstance(sceneAwarnessValidator)
            .AsSingle();

        Container.BindFactory<UnityEngine.Object, AnchorScript, AnchorScript.Factory>()
            .FromFactory<AnchorObjectFactory>();

    }
}
