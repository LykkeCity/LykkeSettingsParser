using System.Threading;
using System.Threading.Tasks;

namespace Lykke.SettingsReader
{
    public abstract class ReloadingManagerBase<TSettings> : IReloadingManager<TSettings>
    {
        private readonly ReaderWriterLockSlim _sync = new ReaderWriterLockSlim();

        private Task<TSettings> _currentTask;

        protected abstract Task<TSettings> Load();

        public bool HasLoaded => CurrentTask.Status == TaskStatus.RanToCompletion;

        public TSettings CurrentValue => CurrentTask.Result;

        protected Task<TSettings> CurrentTask => Load(reload: false);

        public Task<TSettings> Reload() => Load(reload: true);

        private Task<TSettings> Load(bool reload)
        {
            bool CheckCurrentTask() => _currentTask != null && !(_currentTask.IsCompleted && reload);

            try
            {
                _sync.EnterReadLock();

                if (CheckCurrentTask())
                {
                    return _currentTask;
                }
            }
            finally
            {
                _sync.ExitReadLock();
            }

            try
            {
                _sync.EnterWriteLock();

                // double check

                if (CheckCurrentTask())
                {
                    return _currentTask;
                }

                return _currentTask = Load();
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }
    }
}