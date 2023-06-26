

namespace BL.BesnesLogic.IServices
{
    public interface IService<T> where T : class
    {
        Task<bool> Add(T entity);
        Task<bool> AddRange(List<T> entity);
        Task<bool> EditRange(List<T> entity);
        Task<bool> Edit(T entity);
        Task<bool> DeleteObjectBy(Expression<Func<T, bool>> where);
        Task<bool> Delete(T entity);
        Task<bool> DeleteListBy(Expression<Func<T, bool>> where);
        Task<T> GetByIncloadList(Expression<Func<T, bool>> where, List<Expression<Func<T, object>>> Incload);
        Task<T> GetObjectBy(Expression<Func<T, bool>> expression);
        Task<bool> Any(Expression<Func<T, bool>> expression);
        Task<bool> EditObjectProperty(T entity, Expression<Func<T, object>> property);
        Task<bool> EditObjectProperteis(T entity, List<LambdaExpression> propertyExpression);
        Task<bool> EditListProperty(List<T> entity, List<LambdaExpression> propertyExpression);
        Task<List<T>> GetListBy(Expression<Func<T, bool>> where);
        Task<List<T>> GetAll();
        Task<List<T>> CallStoredProcedure(string Query);
        Task<bool> DeleteList(List<T> list);
        Task<T> Find(int id);
        Task<int> Count();
        Task<int> CountWhere(Expression<Func<T, bool>> where);
    }
}
