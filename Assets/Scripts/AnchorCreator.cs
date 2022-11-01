using System;
using System.Threading.Tasks;
using UnityEngine;

public interface AnchorCreator
{
    public void createNativeAnchor(GameObject gameObject);

    public Task<Result> createCloudAnchor(GameObject gameObject, int propIndex);


    public class Result
    {

        public class Success : Result
        {
            public string anchorIdentifier { get; }

            public Success(string anchorIdentifier)
            {
                this.anchorIdentifier = anchorIdentifier;
            }
        }

        public class Failure : Result
        {

            public Exception? exception { get; }

            public Failure(Exception? exception = null)
            {
                this.exception = exception;
            }
        }
    }
}
