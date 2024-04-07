namespace MT_app.business.Services
{
    public interface IBaseService<T>
    {
        public Task Save(T t);
        public Task<List<T>> FindAll();
        public Task<T?> FindById(long? id);
        public Task Delete(T t);
        public Task Update(T t);
    }
}
