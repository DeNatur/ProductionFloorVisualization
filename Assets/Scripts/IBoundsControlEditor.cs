using UniRx;
public interface IBoundsControlEditor
{
    public abstract void enableBoundsControl();

    public abstract void disableBoundsControl();
}

public interface IBoundsControlProvider
{
    public IReadOnlyReactiveProperty<bool> areBoundsEnabled { get; }
}

public class BoundsControlRepository : IBoundsControlEditor, IBoundsControlProvider
{

    public IReadOnlyReactiveProperty<bool> areBoundsEnabled => _areBoundsEnabled;

    private readonly ReactiveProperty<bool> _areBoundsEnabled = new ReactiveProperty<bool>();

    public void enableBoundsControl()
    {
        _areBoundsEnabled.Value = true;
    }

    public void disableBoundsControl()
    {
        _areBoundsEnabled.Value = false;
    }
}