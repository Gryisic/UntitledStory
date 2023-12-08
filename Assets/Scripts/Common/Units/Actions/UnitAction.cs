using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.Units.Actions
{
    public abstract class UnitAction
    {
        public abstract void Cancel();

        public abstract UniTask ExecuteAsync(CancellationToken token);
    }
}