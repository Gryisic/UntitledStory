using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.Units.Actions
{
    public abstract class UnitAction
    {
        public abstract UniTask ExecuteAsync(CancellationToken token);
    }
}