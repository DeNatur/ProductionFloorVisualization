public class UserMenuPresenter
{
    private readonly IObjectsCreator _objectCreator;
    private readonly IBoundsControlVisibilityEditor _boundsControlEditor;

    public UserMenuPresenter(IObjectsCreator objectCreator, IBoundsControlVisibilityEditor boundsControlEditor)
    {
        _objectCreator = objectCreator;
        _boundsControlEditor = boundsControlEditor;
    }

    public void enableBoundsControl()
    {
        _boundsControlEditor.enableBoundsControlVisibility();
    }

    public void disableBoundControls()
    {
        _boundsControlEditor.disableBoundsControlVisibility();
    }

    public void createNewMachine(int index)
    {
        _objectCreator.createNewMachine(index);
    }
}
