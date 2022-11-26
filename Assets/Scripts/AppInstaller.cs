using Microsoft.Azure.SpatialAnchors.Unity;
using Zenject;

public class AppInstaller : MonoInstaller
{

    public AzureSessionCoordinator sessionCoordinator;
    public AddAnchorUseCase addAnchorUseCase;
    public SpatialAnchorManager cloudManager;
    public ObjectsCreatorImpl objectsCreator;
    public SceneAwarnessValidator sceneAwarnessValidator;

    public override void InstallBindings()
    {
        Container.Bind<IAnchorsRepository>()
            .To<AzureAnchorsReporitory>()
            .AsSingle();

        Container.Bind<IAddAnchorUseCase>()
            .To<AddAnchorUseCase>()
            .AsSingle();

        Container.Bind<IRemoveAnchorUseCase>()
            .To<RemoveAnchorUseCase>()
            .AsSingle();

        Container.Bind<AzureSessionCoordinator>()
            .FromInstance(sessionCoordinator)
            .NonLazy();

        Container.Bind<SpatialAnchorManager>()
            .FromInstance(cloudManager)
            .AsSingle();

        Container.Bind<IObjectsCreator>()
            .FromInstance(objectsCreator)
            .AsSingle();

        Container.Bind<IAwarnessValidator>()
            .FromInstance(sceneAwarnessValidator)
            .AsSingle();

        Container.BindInterfacesTo<AzureCloudManager>()
            .AsSingle();

        Container.Bind<IGameObjectEditor>()
            .To<GameObjectEditorImpl>()
            .AsSingle();

        Container.Bind<IAnchorLocator>()
            .To<AzureAnchorLocator>()
            .AsSingle();

        Container.BindInterfacesTo<BoundsControlVisibilityRepository>()
            .AsSingle();

        Container.Bind<UserMenuPresenter>()
            .AsSingle();

        Container.Bind<UserMenuView>()
            .AsSingle();

        Container.Bind<IMachineInfoRepository>()
            .To<MachineInfoRepository>()
            .AsSingle();

        Container.BindFactory<int, MachinePresenter, MachinePresenter.Factory>();

        Container.BindFactory<UnityEngine.Object, MachinePresenter, MachineView, MachineView.Factory>()
            .FromFactory<MachineViewFactory>();

    }
}
