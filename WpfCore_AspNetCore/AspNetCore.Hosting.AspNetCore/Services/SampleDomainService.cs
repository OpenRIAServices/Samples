using OpenRiaServices.Server;
using SimpleApplication.Web.Model;

namespace SimpleApplication.Web
{
    [EnableClientAccess]
    public class SampleDomainService : DomainService
    {
        static readonly List<RangeItem> _items = new List<RangeItem>
            {
                new RangeItem() {Id = 1, Text =  "nr 1"},
                new RangeItem() {Id = 2, Text =  "nr 2"},
                new RangeItem() {Id = 3, Text =  "nr 3"},
                new RangeItem() {Id = 4, Text =  "nr 4"},
                new RangeItem() {Id = 5, Text =  "nr 5"},
            };

        private static TimeSpan _lastDelay;

        private Task Delay(int milliseconds)
        {
            return Task.Delay(milliseconds);
        }

        private Task Delay(TimeSpan delay)
        {
            return Task.Delay(delay);
        }

        [Query(HasSideEffects = true)]
        public IQueryable<RangeItem> GetRange()
        {
            return _items.AsQueryable();
        }

        public void UpdateRange(RangeItem range)
        {

        }

        /// Query returing a queryable range in a task
        /// </summary>
        /// <returns></returns>
        [Invoke(HasSideEffects = true)]
        public IEnumerable<RangeItem> GetRangeWithComplexParameterPOST(ComplexType2 ri, int _integer, string str)
        {
            return _items;
        }

        [Invoke(HasSideEffects = false)]
        public IEnumerable<RangeItem> GetRangeWithComplexParameterGET(ComplexType2 ri, int _integer, string str)
        {
            return _items;
        }

        /// <summary>
        /// Query returing a queryable range in a task
        /// </summary>
        /// <returns></returns>
        public Task<IQueryable<RangeItem>> GetQueryableRangeTaskAsync()
        {
            return Delay(1)
               .ContinueWith(_ =>
                   _items.AsQueryable()
               );
        }

        /// <summary>
        /// Single item Query throwing exception directly
        /// </summary>
        /// <returns></returns>
        public Task<IQueryable<RangeItem>> GetQueryableRangeWithExceptionFirst()
        {
            throw new DomainException("GetQueryableRangeWithExceptionFirst", 23);
        }

        /// <summary>
        /// Single item Query throwing exception in task
        /// </summary>
        /// <returns></returns>
        public Task<IQueryable<RangeItem>> GetQueryableRangeWithExceptionTask()
        {
            return Delay(2)
                .ContinueWith<IQueryable<RangeItem>>(_ =>
                {
                    throw new DomainException("GetQueryableRangeWithExceptionFirst", 24);
                });
        }



        /// <summary>
        /// Single item Query throwing exception in task
        /// </summary>
        /// <returns></returns>
        public IQueryable<RangeItem> GetRangeWithNormalException()
        {
            throw new Exception("Not allowed");
        }

        //[RequiresRole("DUMMY")]
        [RequiresAuthentication]
        public IQueryable<RangeItem> GetRangeWithNotAuthorized()
        {
            return null;
        }

        /// <summary>
        /// Query returing a single item in a task
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Query(HasSideEffects = true, IsComposable = false)]
        public Task<RangeItem> GetRangeByIdTaskAsync(int id)
        {
            return Delay(1)
                .ContinueWith(_ =>
                    _items.FirstOrDefault(a => a.Id == id)
                );
        }

        /// <summary>
        /// Single item Query throwing exception directly
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Query(HasSideEffects = true, IsComposable = false)]
        public Task<RangeItem> GetRangeByIdWithExceptionFirst(int id)
        {
            throw new DomainException("GetRangeByIdWithExceptionTask", 23);
        }

        /// <summary>
        /// Single item Query throwing exception in task
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<RangeItem> GetRangeByIdWithExceptionTask(int id)
        {
            await Delay(1);
            throw new DomainException("GetRangeByIdWithExceptionTask", 24);
        }

        /// <summary>
        /// Sleep the specified delay and then set lastdelay
        /// </summary>
        /// <remarks>
        /// Tests invoke returning simple (void) Task 
        /// </remarks>
        /// <param name="delay">The delay.</param>
        /// <returns></returns>
        [Invoke(HasSideEffects = true)]
        public Task SleepAndSetLastDelay(TimeSpan delay)
        {
            return Delay(delay)
               .ContinueWith(_ =>
               {
                   _lastDelay = delay;
               });
        }

        /// <summary>
        /// Get delay set by SleepAndSetLastDelay
        /// </summary>
        /// <returns>delay set by SleepAndSetLastDelay</returns>
        public TimeSpan GetLastDelay()
        {
            return _lastDelay;
        }

        /// <summary>
        /// Adds one to the number sent by client.
        /// </summary>
        /// <remarks>
        /// Tests invoke returning Task{reference type}
        /// </remarks>
        public async Task<string> GreetTaskAsync(string client)
        {
            await Delay(1);
            return string.Format("Hello {0}", client);
        }

        /// <summary>
        /// Adds one to the number sent by client.
        /// </summary>
        /// <remarks>
        /// Tests invoke returning Task{value type}
        /// </remarks>
        public Task<int> AddOneAsync(int number)
        {
            return Delay(1)
                .ContinueWith(t => number + 1);
        }

        /// <summary>
        /// Adds one to the number sent by client.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <remarks>
        /// Tests invoke returning Task{Nullable{T}}
        /// </remarks>
        public Task<int?> AddNullableOne(int? number)
        {
            return Delay(1)
                .ContinueWith(t => number + 1);
        }


        /// <summary>
        /// Invoke throwing exception directly
        /// </summary>
        /// <returns></returns>
        public Task InvokeWithExceptionFirst()
        {
            throw new DomainException("InvokeWithExceptionFirst", 24);
        }

        /// <summary>
        /// Invoke throwing exception directly
        /// </summary>
        /// <returns></returns>
        public Task InvokeWithExceptionTask(int errorCode)
        {
            return Delay(2)
                .ContinueWith(_ =>
                {
                    throw new DomainException("InvokeWithExceptionTask", errorCode);
                });
        }
    }
}