using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace Lykke.SettingsReader.Test
{
    public class NestedReloadingManagerTest
    {
        [Fact]
        public async Task Reload__Should_Not_Fail_After_Exception_On_Previous_Call()
        {
            var failureManager = new AsyncAwaitFailureManager
            (
                new object(),
                new Exception(),
                new object()
            );
            
            var nestedManager = failureManager.Nested(x => x);

            await nestedManager.Reload();

            await Assert.ThrowsAsync<Exception>(() => nestedManager.Reload());

            // Exception should not be thrown here
            await nestedManager.Reload();
        }

        public class AsyncAwaitFailureManager : ReloadingManagerBase<object>
        {
            private readonly Queue<object> _results;

            
            public AsyncAwaitFailureManager(
                params object[] results)
            {
                _results = new Queue<object>(results);
            }
            
            
            protected override async Task<object> Load()
            {
                // It's important to use async-await in this test
                await Task.Delay(1);
                
                var nextResult = _results.Dequeue();
                
                if (nextResult is Exception e)
                {
                    throw e;
                }
                
                return nextResult;
            }
        }
    }
}