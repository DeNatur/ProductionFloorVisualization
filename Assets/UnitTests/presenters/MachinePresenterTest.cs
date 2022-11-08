using NSubstitute;
using NUnit.Framework;
using UniRx;


public class MachinePresenterTest
{

    static IAddAnchorUseCase addAnchorUseCase = Substitute.For<IAddAnchorUseCase>();
    static IRemoveAnchorUseCase removeAnchorUseCase = Substitute.For<IRemoveAnchorUseCase>();
    static IBoundsControlVisibilityProvider boundsControlVisibilityProvider = Substitute.For<IBoundsControlVisibilityProvider>();


    static MachinePresenter subject;

    static MachinePresenter.State latestState = null;

    int machineIndex = 0;

    [SetUp]
    public void setUp()
    {
        subject = new MachinePresenter(machineIndex, addAnchorUseCase, removeAnchorUseCase, boundsControlVisibilityProvider);
        subject.state.Subscribe((state) => latestState = state);
    }

    [TestFixture]
    public class InitialState : MachinePresenterTest
    {
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
}

