using Managix.Redis.Abstractions;
using Managix.Redis.Extensions;
using Managix.Redis.Helpers;
using StackExchange.Redis;

namespace Managix.Redis.Implementations
{
    public partial class RedisDatabase : IRedisDatabase
    {
        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetByTagAsync<T>(string tag, CommandFlags commandFlags = CommandFlags.None)
        {
            var tagKey = TagHelper.GenerateTagKey(tag);

            var keys = await SetMembersAsync<string>(tagKey, commandFlags).ConfigureAwait(false);

            var result = await GetAllAsync<T>(keys).ConfigureAwait(false);

            return result.Values;
        }

        /// <inheritdoc/>
        public async Task<long> RemoveByTagAsync(string tag, CommandFlags commandFlags = CommandFlags.None)
        {
            var tagKey = TagHelper.GenerateTagKey(tag);

            var keys = await SetMembersAsync<string>(tagKey, commandFlags).ConfigureAwait(false);

            return await RemoveAllAsync(keys, commandFlags).ConfigureAwait(false);
        }

        private Task<bool> ExecuteAddWithTags(
            string key,
            HashSet<string> tags,
            Func<IDatabaseAsync, Task<bool>> action,
            When when = When.Always,
            CommandFlags commandFlags = CommandFlags.None)
        {
            var transaction = Database.CreateTransaction();

            TryAddCondition(transaction, when, key);

            foreach (var tagKey in tags.Select(TagHelper.GenerateTagKey))
                transaction.SetAddAsync(tagKey, key.OfValueSize(Serializer, _maxValueLength, tagKey), commandFlags);

            action(transaction);

            return transaction.ExecuteAsync(commandFlags);
        }

        private static void TryAddCondition(ITransaction transaction, When when, string key)
        {
            var condition = when switch
            {
                When.NotExists => Condition.KeyNotExists(key),
                When.Exists => Condition.KeyExists(key),
                _ => null
            };

            if (condition is null)
                return;

            transaction.AddCondition(condition);
        }
    }
}
