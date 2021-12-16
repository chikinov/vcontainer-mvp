using System.Threading;
using System.Threading.Tasks;

namespace VContainer.Unity.MVP
{
    public sealed class DefaultAnimation : Animation
    {
        private CancellationTokenSource cancellationTokenSource;

        public override bool IsDone =>
            cancellationTokenSource == null ||
            cancellationTokenSource.Token.IsCancellationRequested;

        public override IAnimation Play()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            _ = PlayAsync(cancellationTokenSource.Token);

            return this;
        }

        private async Task PlayAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            if (cancellationToken.IsCancellationRequested) return;

            cancellationTokenSource = null;

            onComplete?.Invoke();
            onComplete = null;
        }

        public override void Stop()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = null;

            onComplete = null;
        }

        public override void Complete()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = null;

            onComplete?.Invoke();
            onComplete = null;
        }
    }
}
