using NSubstitute;
using NUnit.Framework;
using UniRx;


public class MachinePresenterTest
{

    IAddAnchorUseCase addAnchorUseCase = Substitute.For<IAddAnchorUseCase>();
    IRemoveAnchorUseCase removeAnchorUseCase = Substitute.For<IRemoveAnchorUseCase>();
    IBoundsControlVisibilityProvider boundsControlVisibilityProvider = Substitute.For<IBoundsControlVisibilityProvider>();


    MachinePresenter subject;

    MachinePresenter.State latestState = null;

    int machineIndex = 0;

    [SetUp]
    public void setUp()
    {
        subject = new MachinePresenter(machineIndex, addAnchorUseCase, removeAnchorUseCase, boundsControlVisibilityProvider);
        subject.state.Subscribe((state) => latestState = state);
    }

    [Test]
    public void initiallyAddAnchorIsVisible()
    {
        Assert.True(latestState.isAddAnchorVisible);
    }

    [Test]
    public void initiallyTapToPlaceIsVisible()
    {
        Assert.True(latestState.isTapToPlaceVisible);
    }

    [Test]
    public void initiallyDeleteMachineIsVisible()
    {
        Assert.True(latestState.isDeleteMachineVisible);
    }

    [Test]
    public void initiallyRemoveAnchorIsNotVisible()
    {
        Assert.False(latestState.isRemoveAnchorVisible);
    }


    [Test]
    public void initiallyBoundControlsAreNotVisible()
    {
        Assert.False(latestState.isRemoveAnchorVisible);
    }
}
