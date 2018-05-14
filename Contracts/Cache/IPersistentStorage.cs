namespace Contracts.Cache
{
    public interface IPersistentStorage
    {
		T Load<T>(string key);
		void Clear();
        void Save(object itemToSave, string key);
    }
}
