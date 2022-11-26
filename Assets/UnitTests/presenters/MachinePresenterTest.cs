using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using UniRx;


public class MachinePresenterTest
{

    IAddAnchorUseCase addAnchorUseCase = Substitute.For<IAddAnchorUseCase>();
    IRemoveAnchorUseCase removeAnchorUseCase = Substitute.For<IRemoveAnchorUseCase>();
    IBoundsControlVisibilityProvider boundsControlVisibilityProvider = Substitute.For<IBoundsControlVisibilityProvider>();
    IMachineInfoRepository machineInfoRepository = Substitute.For<IMachineInfoRepository>();
    private readonly ReactiveProperty<bool> areBoundsEnabled = new ReactiveProperty<bool>(false);
    private readonly ReactiveProperty<IMachineInfoRepository.MachineInfo?> machineInfoFlow =
            new ReactiveProperty<IMachineInfoRepository.MachineInfo?>(null);

    MachinePresenter subject;

    MachinePresenter.State latestState = null;

    int machineIndex = 0;

    [SetUp]
    public void setUp()
    {
        machineInfoRepository.getMachineInfo(machineIndex).Returns(machineInfoFlow);
        boundsControlVisibilityProvider.isBoundsVisibilityEnabled.Returns(areBoundsEnabled);
        subject = new MachinePresenter(
            machineIndex,
            addAnchorUseCase,
            removeAnchorUseCase,
            boundsControlVisibilityProvider,
            machineInfoRepository
        );
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
            areBoundsEnabled.Value = true;
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

        [Test]
        public void boundsControlAreNotVisible()
        {
            Assert.False(latestState.areBoundControlsVisible);
        }
    }

    [TestFixture]
    public class SetsAnchorNotCreatedState : MachinePresenterTest
    {

        [SetUp]
        public new void setUp()
        {
            areBoundsEnabled.Value = true;
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

        [Test]
        public void boundsControlAreNotVisible()
        {
            Assert.True(latestState.areBoundControlsVisible);
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

    [TestFixture]
    public class BoundsControlAreEnabled : MachinePresenterTest
    {

        [SetUp]
        public new void setUp()
        {
            areBoundsEnabled.Value = true;
        }

        [Test]
        public void boundsAreVisible()
        {
            Assert.True(latestState.areBoundControlsVisible);
        }
    }

    [TestFixture]
    public class AnchorsCreatedAndBoundsControlAreEnabled : MachinePresenterTest
    {

        [SetUp]
        public new void setUp()
        {
            areBoundsEnabled.Value = true;
            subject.setAnchorCreatedState();
        }

        [Test]
        public void boundsAreNotVisible()
        {
            Assert.False(latestState.areBoundControlsVisible);
        }
    }

    [TestFixture]
    public class BoundsControlAreEnabledAndDisabled : MachinePresenterTest
    {

        [SetUp]
        public new void setUp()
        {
            areBoundsEnabled.Value = true;
            areBoundsEnabled.Value = false;
        }

        [Test]
        public void boundsAreNotVisible()
        {
            Assert.False(latestState.areBoundControlsVisible);
        }
    }

    [TestFixture]
    public class MachineInfoIsEmmited : MachinePresenterTest
    {
        IMachineInfoRepository.MachineInfo mockedMachine = new IMachineInfoRepository.MachineInfo
        {
            hallId = 0,
            name = "ASP kolumna robocza",
            efficiency = 100,
            status = "Off",
            symbol = "10000-2",
            technicalExaminationDate = DateTime.Parse("2021-12-31T12:00:00.000+00:00")
        };

        [SetUp]
        public new void setUp()
        {
            machineInfoFlow.Value = mockedMachine;
        }

        [Test]
        public void hallIdIsMapped()
        {
            Assert.AreEqual(
                latestState.machineInfo.Value.hallId,
                mockedMachine.hallId
            );
        }

        [Test]
        public void machineNameIsMapped()
        {
            Assert.AreEqual(
                latestState.machineInfo.Value.name,
                mockedMachine.name
            );
        }

        [Test]
        public void machineEfficiencyIsMapped()
        {
            Assert.AreEqual(
                latestState.machineInfo.Value.efficiency,
                mockedMachine.efficiency
            );
        }

        [Test]
        public void statusIsMapped()
        {
            Assert.AreEqual(
                latestState.machineInfo.Value.status,
                mockedMachine.status
            );
        }

        [Test]
        public void symbolIsMapped()
        {
            Assert.AreEqual(
                latestState.machineInfo.Value.symbol,
                mockedMachine.symbol
            );
        }

        [Test]
        public void technicalExaminationDateIsMapped()
        {
            Assert.AreEqual(
                latestState.machineInfo.Value.technicalExaminationDate,
                mockedMachine.technicalExaminationDate
            );
        }
    }
}

