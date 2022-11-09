using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
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

    [TestFixture]
    public class SetsAnchorCreatedState : MachinePresenterTest
    {

        [SetUp]
        public new void setUp()
        {
            subject.setAnchorCreatedState();
        }

        [Test]
        public void addAnchorIsNotVisible()
        {
            Assert.False(latestState.isAddAnchorVisible);
        }

        [Test]
        public void tapToPlaceIsNotVisible()
        {
            Assert.False(latestState.isTapToPlaceVisible);
        }

        [Test]
        public void deleteMachineIsNotVisible()
        {
            Assert.False(latestState.isDeleteMachineVisible);
        }

        [Test]
        public void removeAnchorIsVisible()
        {
            Assert.True(latestState.isRemoveAnchorVisible);
        }
    }

    [TestFixture]
    public class SetsAnchorNotCreatedState : MachinePresenterTest
    {

        [SetUp]
        public new void setUp()
        {
            subject.setAnchorNotCreatedState();
        }

        [Test]
        public void addAnchorIsVisible()
        {
            Assert.True(latestState.isAddAnchorVisible);
        }

        [Test]
        public void tapToPlaceIsVisible()
        {
            Assert.True(latestState.isTapToPlaceVisible);
        }

        [Test]
        public void deleteMachineIsVisible()
        {
            Assert.True(latestState.isDeleteMachineVisible);
        }

        [Test]
        public void removeAnchorIsNotVisible()
        {
            Assert.False(latestState.isRemoveAnchorVisible);
        }
    }

    [TestFixture]
    public class SetAnchorSuccess : MachinePresenterTest
    {
        private Task setUpTask;

        private UnityEngine.GameObject mockedGO;


        [SetUp]
        public new void setUp()
        {
            addAnchorUseCase.createAzureAnchor(mockedGO, machineIndex)
                .Returns(Task.FromResult(true));
            setUpTask = Task.Run(async () =>
            {
                await subject.setAnchor(mockedGO);
            });
        }


        [Test]
        public async void invokesAddAnchorUseCase()
        {
            await setUpTask;
            await addAnchorUseCase.Received().createAzureAnchor(mockedGO, machineIndex);
        }

        [Test]
        public async void addAnchorIsNotVisible()
        {
            await setUpTask;
            Assert.False(latestState.isAddAnchorVisible);
        }

        [Test]
        public async void tapToPlaceIsNotVisible()
        {
            await setUpTask;
            Assert.False(latestState.isTapToPlaceVisible);
        }

        [Test]
        public async void deleteMachineIsNotVisible()
        {
            await setUpTask;
            Assert.False(latestState.isDeleteMachineVisible);
        }

        [Test]
        public async void removeAnchorIsVisible()
        {
            await setUpTask;
            Assert.True(latestState.isRemoveAnchorVisible);
        }
    }

    [TestFixture]
    public class SetAnchorFailure : MachinePresenterTest
    {
        private Task setUpTask;

        private UnityEngine.GameObject mockedGO;


        [SetUp]
        public new void setUp()
        {
            addAnchorUseCase.createAzureAnchor(mockedGO, machineIndex)
                .Returns(Task.FromResult(false));
            setUpTask = Task.Run(async () =>
            {
                await subject.setAnchor(mockedGO);
            });
        }

        [Test]
        public async void addAnchorIsVisible()
        {
            await setUpTask;
            Assert.True(latestState.isAddAnchorVisible);
        }

        [Test]
        public async void tapToPlaceIsVisible()
        {
            await setUpTask;
            Assert.True(latestState.isTapToPlaceVisible);
        }

        [Test]
        public async void deleteMachineIsVisible()
        {
            await setUpTask;
            Assert.True(latestState.isDeleteMachineVisible);
        }

        [Test]
        public async void removeAnchorIsNotVisible()
        {
            await setUpTask;
            Assert.False(latestState.isRemoveAnchorVisible);
        }
    }

    [TestFixture]
    public class RemovesAnchor : MachinePresenterTest
    {
        private Task setUpTask;

        private UnityEngine.GameObject mockedGO;


        [SetUp]
        public new void setUp()
        {
            setUpTask = Task.Run(async () =>
            {
                subject.setAnchorCreatedState();
                await subject.removeAnchor(mockedGO);
            });
        }

        [Test]
        public void invokesRemoveAnchorUseCase()
        {
            Task.Run(async () =>
            {
                await setUpTask;
                await removeAnchorUseCase.Received().removeAzureAnchor(mockedGO);
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void addAnchorIsVisible()
        {
            Task.Run(async () =>
            {
                await setUpTask;
                Assert.True(latestState.isAddAnchorVisible);
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void tapToPlaceIsVisible()
        {
            Task.Run(async () =>
            {
                await setUpTask;
                Assert.True(latestState.isTapToPlaceVisible);
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void deleteMachineIsVisible()
        {
            Task.Run(async () =>
            {
                await setUpTask;
                Assert.True(latestState.isDeleteMachineVisible);
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void removeAnchorIsNotVisible()
        {
            Task.Run(async () =>
            {
                await setUpTask;
                Assert.False(latestState.isRemoveAnchorVisible);
            }).GetAwaiter().GetResult();
        }
    }

    [TestFixture]
    public class AnchorNotCreatedTryToDelete : MachinePresenterTest
    {

        bool hasBeenRaised = false;

        [SetUp]
        public new void setUp()
        {
            subject.setAnchorNotCreatedState();
            subject.deleteCurrentMachine += delegate ()
            {
                hasBeenRaised = true;
            };
            subject.delete();
        }

        [Test]
        public void invokesDeleteCurrentMachine()
        {
            Assert.True(hasBeenRaised);
        }
    }

    [TestFixture]
    public class AnchorCreatedTryToDelete : MachinePresenterTest
    {

        bool hasBeenRaised = false;

        [SetUp]
        public new void setUp()
        {
            subject.setAnchorCreatedState();
            subject.deleteCurrentMachine += delegate ()
            {
                hasBeenRaised = true;
            };
            subject.delete();
        }

        [Test]
        public void doesNotInvokeDeleteCurrentMachine()
        {
            Assert.False(hasBeenRaised);
        }
    }

    [TestFixture]
    public class SetsTapToPlaceNotStarted : MachinePresenterTest
    {

        bool hasBeenRaised = false;

        [SetUp]
        public new void setUp()
        {
            subject.setAnchorCreatedState();
            subject.disableTapToPlace += delegate ()
            {
                hasBeenRaised = true;
            };
            subject.setTapToPlaceNotStarted();
        }

        [Test]
        public void invokesDisableTapToPlace()
        {
            Assert.True(hasBeenRaised);
        }
    }
}

