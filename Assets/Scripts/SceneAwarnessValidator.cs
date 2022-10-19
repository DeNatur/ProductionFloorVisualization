using Microsoft.Azure.SpatialAnchors.Unity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class SceneAwarnessValidator : MonoBehaviour
{
    private SpatialAnchorManager _cloudManager;

    private readonly Queue<Action> dispatchQueue = new Queue<Action>();

    // Start is called before the first frame update
    [Inject]
    public void Construct(SpatialAnchorManager cloudManager)
    {
        _cloudManager = cloudManager;
    }

    public async Task validateSceneReadiness()
    {
        while (!_cloudManager.IsReadyForCreate)
        {
            await Task.Delay(330);
            float createProgress = _cloudManager.SessionStatus.RecommendedForCreateProgress;
            QueueOnUpdate(new Action(() => Debug.Log($"Move your device to capture more environment data: {createProgress:0%}")));
        }
    }

    void Update()
    {
        lock (dispatchQueue)
        {
            if (dispatchQueue.Count > 0)
            {
                dispatchQueue.Dequeue()();
            }
        }
    }

    private void QueueOnUpdate(Action updateAction)
    {
        lock (dispatchQueue)
        {
            dispatchQueue.Enqueue(updateAction);
        }
    }
}
