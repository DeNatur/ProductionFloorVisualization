using NSubstitute;
using NUnit.Framework;

public class UserMenuPresenterTest
{

    IObjectsCreator objectsCreator = Substitute.For<IObjectsCreator>();
    IBoundsControlVisibilityEditor boundsControlVisiblityEditor = Substitute.For<IBoundsControlVisibilityEditor>();

    UserMenuPresenter subject;

    [SetUp]
    public void setUp()
    {
        subject = new UserMenuPresenter(objectsCreator, boundsControlVisiblityEditor);
    }

    [TestFixture]
    public class BoundControlsAreEnabled : UserMenuPresenterTest
    {

        [SetUp]
        public new void setUp()
        {
            subject.enableBoundsControl();
        }

        [Test]
        public void invokedEnableBoundControlVisibility()
        {
            boundsControlVisiblityEditor.Received().enableBoundsControlVisibility();
        }
    }

    [TestFixture]
    public class BoundControlsAreDisabled : UserMenuPresenterTest
    {

        [SetUp]
        public new void setUp()
        {
            subject.disableBoundControls();
        }

        [Test]
        public void invokedEnableBoundControlVisibility()
        {
            boundsControlVisiblityEditor.Received().disableBoundsControlVisibility();
        }
    }


    [TestFixture]
    public class NewMachineIsCreated : UserMenuPresenterTest
    {

        int index = 0;

        [SetUp]
        public new void setUp()
        {
            subject.createNewMachine(index);
        }

        [Test]
        public void invokedEnableBoundControlVisibility()
        {
            objectsCreator.Received().createNewMachine(index);
        }
    }
}
