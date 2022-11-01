using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System.Threading.Tasks;

public class RemoveAnchorUseCaseTest
{
    AnchorsRepository anchorsRepository = Substitute.For<AnchorsRepository>();
    AnchorRemover anchorRemover = Substitute.For<AnchorRemover>();
    GameObjectEditor gameObjectEditor = Substitute.For<GameObjectEditor>();
    RemoveAnchorUseCase subject;

    static private UnityEngine.GameObject mockedGO;
    static private string mockedIdentifier = "test";
    private AnchorsRepository.AnchorGameObject mockedAnchorGO = new AnchorsRepository.AnchorGameObject
    {
        identifier = mockedIdentifier,
        gameObject = mockedGO
    };

    [SetUp]
    public void setUp()
    {
        subject = new RemoveAnchorUseCase(anchorsRepository, anchorRemover, gameObjectEditor);
        gameObjectEditor.getName(mockedGO).Returns(mockedIdentifier);
    }

    [TestFixture]
    public class NoAnchor : RemoveAnchorUseCaseTest
    {

        private Task resultTask;

        [SetUp]
        public void setUp()
        {
            anchorsRepository.getAnchor(mockedIdentifier).ReturnsNull();
            resultTask = Task.Run(async () => await subject.removeAzureAnchor(mockedGO));
        }

        [Test]
        public void doesNotDeleteNativeAnchor()
        {
            Task.Run(async () =>
            {
                await resultTask;
                anchorRemover.DidNotReceive().deleteNativeAnchor(mockedGO);
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void doesNotDeleteCloudAnchor()
        {
            Task.Run(async () =>
            {
                await resultTask;
                await anchorRemover.DidNotReceive().deleteCloudAnchor(mockedGO);
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void doesNotRemoveAnchorFromRepository()
        {
            Task.Run(async () =>
            {
                await resultTask;
                anchorsRepository.DidNotReceive().removeAnchor(mockedIdentifier);
            }).GetAwaiter().GetResult();
        }
    }


    [TestFixture]
    public class AnchorExists : RemoveAnchorUseCaseTest
    {

        private Task resultTask;

        [SetUp]
        public void setUp()
        {
            anchorsRepository.getAnchor(mockedIdentifier).Returns(mockedAnchorGO);
            resultTask = Task.Run(async () => await subject.removeAzureAnchor(mockedGO));
        }

        [Test]
        public void deletesNativeAnchor()
        {
            Task.Run(async () =>
            {
                await resultTask;
                anchorRemover.Received().deleteNativeAnchor(mockedGO);
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void deletesCloudAnchor()
        {
            Task.Run(async () =>
            {
                await resultTask;
                await anchorRemover.Received().deleteCloudAnchor(mockedGO);
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void removesAnchorFromRepository()
        {
            Task.Run(async () =>
            {
                await resultTask;
                anchorsRepository.Received().removeAnchor(mockedIdentifier);
            }).GetAwaiter().GetResult();
        }
    }

}
