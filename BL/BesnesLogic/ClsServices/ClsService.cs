
namespace BL.BesnesLogic.ClsServices
{
    public class ClsService<T> : IService<T> where T : class
    {
        //private readonly PharmacyContext _context;

        //public ClsService(PharmacyContext context)
        //{
        //    _context = context;
        //}

        public async Task<bool> Add(T entity)
        {
            try
            {
                using (PharmacyContext _context = new())
                {

                    await _context.Set<T>().AddAsync(entity);
                    if (await _context.SaveChangesAsync() > 0)
                        return true;
                    return false;

                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddRange(List<T> entity)
        {
            try
            {
                using (PharmacyContext _context = new())
                {
                    await _context.Set<T>().AddRangeAsync(entity);
                    if (await _context.SaveChangesAsync() == entity.Count)
                        return true;
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> Any(Expression<Func<T,bool>> expression)
        {
            try
            {
                using (PharmacyContext _context = new())
                {
                    return await _context.Set<T>().AsNoTracking().AnyAsync(expression);
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> EditObjectProperty(T entity, Expression<Func<T, object>> property)
        {
            try
            {
                using (PharmacyContext _context = new())
                {
                    _context.Entry(entity).Property(property).IsModified = true;
                    if (await _context.SaveChangesAsync() > 0)
                        return true;
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> EditObjectProperteis(T entity, List<LambdaExpression> propertyExpression)
        {
            try
            {
                using (PharmacyContext _context = new())
                {
                    foreach (Expression<Func<T, object>> property in propertyExpression)
                    {
                        _context.Entry(entity).Property(property).IsModified = true;
                    }
                    if (await _context.SaveChangesAsync() > 0)
                        return true;
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Delete(T entity)
        {
            try
            {
                using (PharmacyContext _context = new())
                {
                    await Task.Run(() => _context.Set<T>().Remove(entity));
                    if (await _context.SaveChangesAsync() > 0)
                        return true;
                    return false;

                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteObjectBy(Expression<Func<T, bool>> expression)
        {
            try
            {
                T entity = await GetObjectBy(expression);
                if (entity != null)
                {
                    using (PharmacyContext _context = new())
                    {
                        _context.Set<T>().Remove(entity);
                        if (await _context.SaveChangesAsync() > 0)
                            return true;
                    }
                }
                return false;

            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Edit(T entity)
        {
            try
            {

                using (PharmacyContext _context = new())
                {
                    _context.Set<T>().Update(entity);
                    if (await _context.SaveChangesAsync() > 0)
                        return true;
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<T>> GetAll()
        {
            try
            {

                using (PharmacyContext _context = new())
                {
                    return await _context.Set<T>().AsNoTracking().ToListAsync();
                }
            }
            catch
            {
                return new List<T>();
            }
        }
        public async Task<T> GetObjectBy(Expression<Func<T, bool>> expression)
        {
            try
            {
                using (PharmacyContext _context = new())
                {
                    return await _context.Set<T>().FirstOrDefaultAsync(expression);

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<T>> GetListBy(Expression<Func<T, bool>> where)
        {
            try
            {
                using (PharmacyContext _context = new())
                {
                    return await _context.Set<T>().Where(where).ToListAsync();
                };
            }
            catch
            {
                return new List<T>();
            }
        }
        public async Task<bool> EditRange(List<T> entity)
        {
            try
            {
                using (PharmacyContext _context = new())
                {

                    _context.Set<T>().UpdateRange(entity);
                    if (await _context.SaveChangesAsync() == entity.Count)
                        return true;
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<T>> CallStoredProcedure(string Query)
        {
            try
            {

                using (PharmacyContext _context = new())
                {
                    return await _context.Set<T>().FromSqlRaw(Query).ToListAsync();
                }
            }
            catch
            {
                return new List<T>();
            }
        }
        public async Task<bool> DeleteList(List<T> list)
        {
            try
            {
                using (PharmacyContext _context = new())
                {
                    _context.Set<T>().RemoveRange(list);
                    if (await _context.SaveChangesAsync() > 0)
                        return true;

                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteListBy(Expression<Func<T, bool>> where)
        {
            try
            {
                List<T> entity = await GetListBy(where);
                if (entity != null)
                {
                    using (PharmacyContext _context = new())
                    {

                        _context.Set<T>().RemoveRange(entity);
                        if (await _context.SaveChangesAsync() > 0)
                            return true;
                    }
                }
                return false;

            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> EditListProperty(List<T> entity, List<LambdaExpression> propertyExpression)
        {
            try
            {
                using (PharmacyContext _context = new())
                {

                    foreach (var item in entity)
                    {
                        foreach (Expression<Func<T, object>> property in propertyExpression)
                        {
                            _context.Entry(item).Property(property).IsModified = true;
                        }
                    }
                    if (await _context.SaveChangesAsync() > 0)
                        return true;
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<T> Find(int id)
        {
            try
            {
                using (PharmacyContext _context = new())
                {
                    return await _context.Set<T>().FindAsync(id);
                }
            }
            catch
            {
                return null;
            }
        }
        public async Task<T> GetByIncloadList(Expression<Func<T, bool>> where, List<Expression<Func<T, object>>> Incload)
        {
            using (PharmacyContext _context = new())

            {
                var query = _context.Set<T>().Where(where);
                foreach (var incloa in Incload)
                {
                    query = query.Include(incloa);
                }
                return await Task.Run(() => query.FirstOrDefaultAsync());
            }
        }
        public async Task<int> Count()
        {
            using (PharmacyContext _context = new())
            {
                return await _context.Set<T>().CountAsync();
            }
        }
        public async Task<int> CountWhere(Expression<Func<T, bool>> where)
        {
            using (PharmacyContext _context = new())
            {

                return await _context.Set<T>().Where(where).CountAsync();
            }
        }
    }
}
