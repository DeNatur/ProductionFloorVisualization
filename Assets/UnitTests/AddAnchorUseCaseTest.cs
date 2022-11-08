using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System.Threading.Tasks;

public class AddAnchorUseCaseTest
{

    IAnchorsRepository anchorsRepository = Substitute.For<IAnchorsRepository>();
    IAnchorCreator saveAnchor = Substitute.For<IAnchorCreator>();
    IAwarnessValidator validator = Substitute.For<IAwarnessValidator>();
    GameObjectEditor gameObjectEditor = Substitute.For<GameObjectEditor>();
    AddAnchorUseCase subject;

    static private UnityEngine.GameObject mockedGO;
    static private string mockedIdentifier = "test";
    static private int mockedIndex = 0;

    [SetUp]
    public void setUp()
    {
        mockedGO = new UnityEngine.GameObject();
        subject = new AddAnchorUseCase(
            anchorsRepository: anchorsRepository,
            saveAnchor: saveAnchor,
            sceneAwarnessValidator: validator,
            gameObjectEditor: gameObjectEditor
        );
        gameObjectEditor.getName(mockedGO).Returns(mockedIdentifier);
    }

    [TestFixture]
    public class AlreadyCreatedAnchor : AddAnchorUseCaseTest
    {

        private Task<bool> resultTask;
        private IAnchorsRepository.AnchorGameObject mockedAnchorGO
            = new IAnchorsRepository.AnchorGameObject
            {
                identifier = mockedIdentifier,
                gameObject = mockedGO
            };

        [SetUp]

        public new void setUp()
        {
            anchorsRepository.getAnchor(mockedIdentifier)
                .Returns(mockedAnchorGO);
            resultTask = Task.Run(async () => await subject.createAzureAnchor(mockedGO, 0));
        }

        [Test]
        public async void returnsFalse()
        {
            Assert.False(await resultTask);
        }
    }

    [TestFixture]
    public class NewAnchor : AddAnchorUseCaseTest
    {

        private static IAnchorsRepository.AnchorGameObject mockedAnchorGO
            = new IAnchorsRepository.AnchorGameObject
            {
                identifier = mockedIdentifier,
                gameObject = mockedGO
            };

        [SetUp]
        public new void setUp()
        {
            anchorsRepository.getAnchor(mockedIdentifier)
                .ReturnsNull();
        }

        [TestFixture]
        public class CreateCloudAnchorSuccess : AddAnchorUseCaseTest
        {

            private const string newIdentifier = "new";
            private IAnchorCreator.Result anchorResult = new IAnchorCreator.Result.Success(newIdentifier);
            private Task<bool> resultTask;

            [SetUp]
            public new void setUp()
            {
                resultTask = Task.Run(async () =>
                {
                    saveAnchor.createCloudAnchor(mockedGO, mockedIndex)
                        .Returns(Task.FromResult(anchorResult));
                    return await subject.createAzureAnchor(mockedGO, 0);
                });
            }

            [Test]
            public void validatesSceneReadiness()
            {
                Task.Run(async () =>
                {
                    await resultTask;
                    await validator.Received().validateSceneReadiness();
                }).GetAwaiter().GetResult();
            }

            [Test]
            public void createsNativeAnchorOnGameObject()
            {
                Task.Run(async () =>
                {
                    await resultTask;
                    saveAnchor.Received().createNativeAnchor(mockedGO);
                }).GetAwaiter().GetResult();
            }

            [Test]
            public void returnsTrue()
            {
                Task.Run(async () =>
                {
                    var result = await resultTask;
                    Assert.IsTrue(result);
                }).GetAwaiter().GetResult();
            }

            [Test]
            public void setsNewGOName()
            {
                Task.Run(async () =>
                {
                    await resultTask;
                    gameObjectEditor.Received().setName(mockedGO, newIdentifier);
                }).GetAwaiter().GetResult();
            }

            [Test]
            public void savesAnchorGO()
            {
                Task.Run(async () =>
                {
                    await resultTask;
                    anchorsRepository.Received().addAnchor(
                        Arg.Is<IAnchorsRepository.AnchorGameObject>(
                            x => x.gameObject == mockedGO
                            )
                        );
                }).GetAwaiter().GetResult();
            }

            [Test]
            public void savesAnchorIdentifier()
            {
                Task.Run(async () =>
                {
                    await resultTask;
                    anchorsRepository.Received().addAnchor(
                        Arg.Is<IAnchorsRepository.AnchorGameObject>(
                            x => x.identifier == newIdentifier
                            )
                        );
                }).GetAwaiter().GetResult();
            }
        }

        [TestFixture]
        public class CreateCloudAnchorFailure : AddAnchorUseCaseTest
        {

            private const string newIdentifier = "new";
            private IAnchorCreator.Result anchorResult = new IAnchorCreator.Result.Failure();
            private Task<bool> resultTask;

            [SetUp]
            public new void setUp()
            {
                resultTask = Task.Run(async () =>
                {
                    saveAnchor.createCloudAnchor(mockedGO, mockedIndex)
                        .Returns(Task.FromResult(anchorResult));
                    return await subject.createAzureAnchor(mockedGO, 0);
                });
            }

            [Test]
            public void returnsFalse()
            {
                Task.Run(async () =>
                {
                    var result = await resultTask;
                    Assert.IsFalse(result);
                }).GetAwaiter().GetResult();
            }

            [Test]
            public void notSavesAnchorGO()
            {
                Task.Run(async () =>
                {
                    await resultTask;
                    anchorsRepository.DidNotReceive().addAnchor(
                        Arg.Is<IAnchorsRepository.AnchorGameObject>(
                            x => x.gameObject == mockedGO
                            )
                        );
                }).GetAwaiter().GetResult();
            }

            [Test]
            public void notSavesAnchorIdentifier()
            {
                Task.Run(async () =>
                {
                    await resultTask;
                    anchorsRepository.DidNotReceive().addAnchor(
                        Arg.Is<IAnchorsRepository.AnchorGameObject>(
                            x => x.identifier == newIdentifier
                            )
                        );
                }).GetAwaiter().GetResult();
            }
        }

    }

}
