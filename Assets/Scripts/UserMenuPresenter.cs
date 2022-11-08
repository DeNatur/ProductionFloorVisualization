public class UserMenuPresenter
{
    private readonly IObjectsCreator _objectCreator;
    private readonly IBoundsControlEditor _boundsControlEditor;

    public UserMenuPresenter(IObjectsCreator objectCreator, IBoundsControlEditor boundsControlEditor)
    {
        _objectCreator = objectCreator;
        _boundsControlEditor = boundsControlEditor;
    }

    public void enableBoundsControl()
    {
        _boundsControlEditor.enableBoundsControl();
    }

    public void disableBoundControls()
    {
        _boundsControlEditor.disableBoundsControl();
    }

    public void createNewMachine(int index)
    {
        _objectCreator.createNewMachine(index);
    }
}
