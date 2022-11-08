using UniRx;
public interface IBoundsControlVisibilityEditor
{
    public abstract void enableBoundsControlVisibility();

    public abstract void disableBoundsControlVisibility();
}

public interface IBoundsControlVisibilityProvider
{
    public IReadOnlyReactiveProperty<bool> isBoundsVisibilityEnabled { get; }
}

public class BoundsControlVisibilityRepository : IBoundsControlVisibilityEditor, IBoundsControlVisibilityProvider
{

    public IReadOnlyReactiveProperty<bool> isBoundsVisibilityEnabled => _areBoundsEnabled;

    private readonly ReactiveProperty<bool> _areBoundsEnabled = new ReactiveProperty<bool>(false);

    public void enableBoundsControlVisibility()
    {
        _areBoundsEnabled.Value = true;
    }

    public void disableBoundsControlVisibility()
    {
        _areBoundsEnabled.Value = false;
    }
}