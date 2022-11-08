using NUnit.Framework;
using UniRx;
public class BoundsControlVisibilityRepositoryTest
{

    static BoundsControlVisibilityRepository subject;

    static bool? currentResultValue = null;

    [SetUp]
    public void setUp()
    {
        subject = new BoundsControlVisibilityRepository();
        subject.isBoundsVisibilityEnabled.Subscribe(x => { currentResultValue = x; });
    }

    [Test]
    public void initialStateIsFalse()
    {
        Assert.False(currentResultValue);
    }

    [TestFixture]
    public class EnablesBoundsControl
    {

        [SetUp]

        public void setUp()
        {
            subject.enableBoundsControlVisibility();
        }

        [Test]
        public void boundsAreVisible()
        {
            Assert.True(currentResultValue);
        }


        [TestFixture]
        public class DisablesBoundsControl
        {

            [SetUp]

            public void setUp()
            {
                subject.disableBoundsControlVisibility();
            }

            [Test]
            public void boundsAreNotVisible()
            {
                Assert.False(currentResultValue);
            }
        }
    }
}
