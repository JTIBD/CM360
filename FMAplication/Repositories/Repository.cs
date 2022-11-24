using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using FMAplication.Attributes;
using FMAplication.common;
using FMAplication.Core;
using FMAplication.Domain.DailyActivities;
using FMAplication.Domain.Organizations;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Domain.WorkFlows;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using X.PagedList;

namespace FMAplication.Repositories
{
    // By: Ashiquzzaman;
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : AuditableEntity<int>
    {
        public IQueryable<TEntity> GetAllIncludeStrFormat
        (Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
        {
            includeProperties = includeProperties ?? string.Empty;
            //IQueryable<TEntity> query = _dbContext.Set<T>();
            var query = DbSet.AsNoTracking().AsQueryable();

            if (filter != null) query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split
                (new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (orderBy != null) query = orderBy(query);

            if (skip.HasValue) query = query.Skip(skip.Value);

            if (take.HasValue) query = query.Take(take.Value);

            return query;
        }

        public string GetTableName()
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            //var schema = entityType.GetSchema();
            return entityType.GetTableName();
        }

        #region CONFIG

        private DbContext _context;
        private bool _disposed;

        public bool ShareContext { get; set; }


        public Repository(DbContext context)
        {
            _context = context;
            //Context.Database.CommandTimeout = 100000;
            // DbInterception.Add(new AzRInterceptor());
        }

        protected DbSet<TEntity> DbSet
        {
            get => _context.Set<TEntity>();
            set => value = _context.Set<TEntity>();
        }

        #endregion

        #region Disposed

        ~Repository()
        {
            Dispose(false);
        }

        /// <summary>
        ///     <see cref="M:System.IDisposable.Dispose" />
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (ShareContext || _context == null) return;
            if (!_disposed)
                if (disposing)
                    if (_context != null)
                    {
                        _context.Dispose();
                        _context = null;
                    }

            _disposed = true;
        }

        #endregion

        #region LINQ

        public IQueryable<TEntity> GetAll()
        {
            return DbSet.AsNoTracking().AsQueryable();
        }
        public IQueryable<TEntity> GetAllActive()
        {
            return DbSet.AsNoTracking().AsQueryable().Where(x=>x.Status != Status.InActive);
        }

        public IQueryable<TEntity> FindAllInclude
        (Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate
            (DbSet.AsNoTracking().AsQueryable().Where(predicate),
                (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<TEntity> GetAllInclude
            (params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate
                (DbSet.AsNoTracking().AsQueryable(), (current, includeProperty) => current.Include(includeProperty));
        }

        public TEntity FindInclude
        (Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate
                    (DbSet.AsNoTracking().AsQueryable(), (current, includeProperty) => current.Include(includeProperty))
                .FirstOrDefault(predicate);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.AsNoTracking().FirstOrDefault(predicate);
        }


        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsNoTracking().AsQueryable();
        }


        public bool IsExist(Expression<Func<TEntity, bool>> predicate)
        {
            var count = DbSet.Count(predicate);
            return count > 0;
        }

        public string CreateId(Expression<Func<TEntity, string>> predicate, string prefix,
            Expression<Func<TEntity, bool>> where = null,
            int returnLength = 9, char fillValue = '0')
        {
            return where != null
                ? DbSet.Where(where).AsNoTracking().Max(predicate).MakeId(prefix, returnLength, fillValue)
                : DbSet.AsNoTracking().Max(predicate).MakeId(prefix, returnLength, fillValue);
        }

        #endregion

        #region SQL

        public async Task<IEnumerable<T>> ExecuteQueryAsyc<T>(string sqlQuery, params object[] parameters)
            where T : class
        {
            return await _context.SqlQueryAsync<T>(sqlQuery, parameters);
        }

        public async Task<IEnumerable<T>> FromSqlRaw<T>(string sqlQuery, params object[] parameters) where T : class
        {
            var setContext = _context.Set<T>();
            return setContext.FromSqlRaw(sqlQuery, parameters);
        }

        public async Task<int> ExecuteSqlCommandAsync(string sqlCommand, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlCommandAsync(sqlCommand, parameters);
        }

        public IEnumerable<dynamic> DynamicListFromSql(string Sql, Dictionary<string, object> Params,
            bool isStoredProcedure = false)
        {
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if (isStoredProcedure)
                    cmd.CommandType = CommandType.StoredProcedure;
                if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

                foreach (var p in Params)
                {
                    var dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = p.Key;
                    dbParameter.Value = p.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var row = new ExpandoObject() as IDictionary<string, object>;
                        for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                            row.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
                        yield return row;
                    }
                }
            }
        }

        public (IList<dynamic> Items, int Total, int TotalFilter) DynamicListFromSql(string sql,
            IList<(string Key, object Value, bool IsOut)> parameters)
        {
            var items = new List<dynamic>();
            int? totalCount = 0;
            int? filteredCount = 0;

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                if (command.Connection.State != ConnectionState.Open) command.Connection.Open();

                foreach (var param in parameters)
                {
                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    if (!param.IsOut)
                    {
                        dbParameter.Value = param.Value;
                    }
                    else
                    {
                        dbParameter.Direction = ParameterDirection.Output;
                        dbParameter.DbType = DbType.Int32;
                    }

                    command.Parameters.Add(dbParameter);
                }

                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var row = new ExpandoObject() as IDictionary<string, object>;
                        for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                            row.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
                        //yield return row;
                        items.Add(row);
                    }
                }

                totalCount = (int?) command.Parameters["TotalCount"].Value;
                filteredCount = (int?) command.Parameters["FilteredCount"].Value;
            }

            return (items, totalCount ?? 0, filteredCount ?? 0);
        }

        public (IList<T> Items, int Total, int TotalFilter) GetDataBySP<T>(string sql,
            IList<(string Key, object Value, bool IsOut)> parameters)
        {
            var items = new List<T>();
            int? totalCount = 0;
            int? filteredCount = 0;

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                if (command.Connection.State != ConnectionState.Open) command.Connection.Open();

                foreach (var param in parameters)
                {
                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    if (!param.IsOut)
                    {
                        dbParameter.Value = param.Value;
                    }
                    else
                    {
                        dbParameter.Direction = ParameterDirection.Output;
                        dbParameter.DbType = DbType.Int32;
                    }

                    command.Parameters.Add(dbParameter);
                }

                command.CommandTimeout = int.MaxValue;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var itemType = typeof(T);
                        var constructor = itemType.GetConstructor(new Type[] { });
                        var instance = constructor.Invoke(new object[] { });
                        var properties = itemType.GetProperties();

                        foreach (var property in properties)
                            if (!reader.IsDBNull(property.Name))
                                property.SetValue(instance, reader[property.Name]);

                        items.Add((T) instance);
                    }
                }

                totalCount = (int?) command.Parameters["TotalCount"].Value;
                filteredCount = (int?) command.Parameters["FilteredCount"].Value;
            }

            return (items, totalCount ?? 0, filteredCount ?? 0);
        }

        #endregion

        #region LINQ ASYNC

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IPagedList<TEntity>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            return await DbSet.AsNoTracking().AsQueryable().ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<IPagedList<TEntity>> FindAllPagedAsync(Expression<Func<TEntity, bool>> predicate,
            int pageNumber, int pageSize)
        {
            return await DbSet.Where(predicate).AsNoTracking().AsQueryable().ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> FindIncludeAsync
        (Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await includeProperties.Aggregate
                    (DbSet.AsNoTracking().AsQueryable(), (current, includeProperty) => current.Include(includeProperty))
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> CreateAsync(TEntity item)
        {
            var tableName = GetTableName();
            DbSet.Add(item);
            await SaveChangesAsync();
            return item;
        }

        public async Task<TEntity> SaveAsync(TEntity item)
        {
            await DbSet.AddAsync(item);
            return item;
        }

        public async Task<List<TEntity>> CreateListAsync(List<TEntity> items)
        {
            DbSet.AddRange(items);
            await SaveChangesAsync();
            return items;
        }

        public async Task<List<TEntity>> SaveListAsync(List<TEntity> items)
        {
            await DbSet.AddRangeAsync(items);
            //await SaveChangesAsync();
            return items;
        }

        public async Task<List<TEntity>> UpdateListAsync(List<TEntity> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var item in items)
            {
                var entry = _context.Entry(item);
                DbSet.Attach(item);
                entry.State = EntityState.Modified;
            }

            var result = await SaveChangesAsync();
            return result > 0 ? items : null;
        }

        public async Task<int> DeleteListAsync(List<TEntity> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var record in items) DbSet.Attach(record);
            DbSet.RemoveRange(items);
            var result = await SaveChangesAsync();
            return result;
        }

        public async Task<int> DeleteAsync(TEntity item)
        {
            DbSet.Attach(item);
            DbSet.Remove(item);
            var result = await SaveChangesAsync();
            return result;
        }


        public async Task<TEntity> UpdateAsync(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var entry = _context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
            var result = await SaveChangesAsync();
            if (result > 0)
                return item;
            return null;
        }

        public void Detach(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var entry = _context.Entry(item);
            entry.State = EntityState.Detached;
            
        }

        public void DetachList(List<TEntity> items)
        {
            if (items == null || !items.Any()) return;
            foreach (var item in items)
            {
                var entry = _context.Entry(item);
                entry.State = EntityState.Detached;
            }
        }

        public async Task<TEntity> CreateOrUpdateAsync(TEntity item)
        {
            var pi = item.GetType().GetProperty("Id");
            var keyFieldId = pi != null ? pi.GetValue(item, null) : -1;
            keyFieldId = keyFieldId == (object) 0 ? -1 : keyFieldId;

            var record = await DbSet.FindAsync(keyFieldId);
            if (record == null)
            {
                var tableName = GetTableName();
                DbSet.Add(item);
            }
            else
            {
                _context.Entry(record).CurrentValues.SetValues(item);
            }

            var result = !ShareContext ? await SaveChangesAsync() : 0;
            return result > 0 ? item : null;
        }

        #region Workflow

        //private async Task<int> InsertIntoWorkFlowLogTable(int rowId, int workflowId, string tableName)
        //{
        //    //---Agenda 1: First Collect All User and insert each user into workflow for

        //    var result = 0;
        //    //Context

        //    var wfConfigContext = _context.Set<WorkFlowConfiguration>();
        //    var wfOrgUserRoleContext = _context.Set<OrganizationUserRole>();
        //    var wfOrgRoleContext = _context.Set<OrganizationRole>();
        //    var userTerrContext = _context.Set<UserTerritoryMapping>();


        //    //get initial role from wfConfig

        //    var wfConfigdata = await wfConfigContext.Include(i => i.OrganizationRole).OrderBy(a => a.sequence)
        //        .FirstOrDefaultAsync(o => o.MasterWorkFlowId == workflowId);

        //    if (wfConfigdata != null)
        //    {
        //        //get orgRoleDetail With Current Position
        //        var orgRole = wfConfigdata.OrganizationRole;
        //        //get all user of that organization role
        //        var orgRoleUsers = await wfOrgUserRoleContext.OrderBy(s => s.UserSequence)
        //            .Where(a => a.OrgRoleId == wfConfigdata.OrgRoleId).Select(a => a.UserId).ToListAsync();


        //        //---------------------------------
        //        //Current User details, becuase current user parent will receive wf. Which parent? it will determine by his/her org role designation

        //        var userTerrNodeIds = await userTerrContext.Where(a => a.UserInfoId == AppIdentity.AppUser.UserId)
        //            .Select(a => a.NodeId).ToListAsync();


        //        var currentUserParentId = await GetCurrentUserParentByNode(userTerrContext, orgRole, userTerrNodeIds);
        //        if (currentUserParentId.Count > 0)
        //            orgRoleUsers.AddRange(currentUserParentId);


        //        //var userData = await wfOrgUserRoleContext.OrderBy(s=> s.UserSequence).Where(a => a.OrgRoleId == wfConfigdata.OrgRoleId).ToListAsync();

        //        if (orgRoleUsers.Count > 0)
        //        {
        //            foreach (var item in orgRoleUsers)
        //            {
        //                var wfLog = new WorkflowLog
        //                {
        //                    RowId = rowId,
        //                    MasterWorkFlowId = workflowId,
        //                    WorkflowStatus = (int) WorkflowStatus.Pending,
        //                    WorkFlowFor = item,
        //                    TableName = tableName,
        //                    OrgRoleId = orgRole.Id
        //                };
        //                var wfLogContext = _context.Set<WorkflowLog>();
        //                wfLogContext.Add(wfLog);
        //            }

        //            result = await SaveChangesAsync();
        //        }
        //        else
        //        {
        //            result = -1;
        //        }
        //    }

        //    return result;
        //}

        private async Task<List<int>> GetCurrentUserParentByNode(DbSet<UserTerritoryMapping> userTerrContext,
            OrganizationRole orgRole, List<int> nodeIds)
        {
            var userId = new List<int>();

            foreach (var item in nodeIds)
            {
                var allParentOfCurrentUserNode = await GetUserIdByParentNode(item, new Dictionary<string, int>());

                var parentNodeOfCurrentUserBasedOnOrgRole = 0;

                if (orgRole.DesignationId == (int) OrgRoleDesignation.Area)
                    parentNodeOfCurrentUserBasedOnOrgRole =
                        allParentOfCurrentUserNode.FirstOrDefault(x => x.Key.StartsWith("A")).Value;
                else if (orgRole.DesignationId == (int) OrgRoleDesignation.Region)
                    parentNodeOfCurrentUserBasedOnOrgRole =
                        allParentOfCurrentUserNode.FirstOrDefault(x => x.Key.StartsWith("R")).Value;
                else if (orgRole.DesignationId == (int) OrgRoleDesignation.National)
                    parentNodeOfCurrentUserBasedOnOrgRole =
                        allParentOfCurrentUserNode.FirstOrDefault(x => x.Key.StartsWith("N")).Value;
                else if (orgRole.DesignationId == (int) OrgRoleDesignation.Territory)
                    parentNodeOfCurrentUserBasedOnOrgRole =
                        allParentOfCurrentUserNode.FirstOrDefault(x => x.Key.StartsWith("T")).Value;

                if (parentNodeOfCurrentUserBasedOnOrgRole > 0)
                {
                    var getUserFromUserTerrUsingNode =
                        await userTerrContext.FirstOrDefaultAsync(
                            a => a.NodeId == parentNodeOfCurrentUserBasedOnOrgRole);
                    if (getUserFromUserTerrUsingNode != null)
                        userId.Add(getUserFromUserTerrUsingNode.UserInfoId);
                }
            }

            return userId;
        }

        public async Task<Dictionary<string, int>> GetUserIdByParentNode(int nodeid, Dictionary<string, int> dict)
        {
            var nodeContext = _context.Set<Node>();


            var checkTerrNode = await nodeContext.FirstOrDefaultAsync(a => a.NodeId == nodeid);

            if (checkTerrNode != null)
            {
                dict.Add(checkTerrNode.Code, nodeid);
                if (checkTerrNode.ParentId != null) await GetUserIdByParentNode(checkTerrNode.ParentId ?? 0, dict);
            }

            return dict;
        }

        public async Task<WorkFlow> IsWfEnabledForEntityGetWf(string tableName)
        {
            //Checking table Availble in WorkflowType
            var contextforWorkflowType = _context.Set<WorkFlowType>();
            var isWorkflowAvailable = await contextforWorkflowType.FirstOrDefaultAsync(a =>
                a.DbTableName.ToUpper() == tableName.ToUpper() && a.IsWorkflowConfigAvailable &&
                a.IsWorkflowDefAvailable);
            if (isWorkflowAvailable != null)
            {
                //get workflow data
                var contextForWorkflow = _context.Set<WorkFlow>();
                var data = await contextForWorkflow.FirstOrDefaultAsync(a => a.WorkflowType == isWorkflowAvailable.Id);
                return data;
            }

            return null;
        }

        public async Task<bool> CheckWfEnabledForEntity(string tableName)
        {
            var contextforWorkflowType = _context.Set<WorkFlowType>();
            var isWorkflowAvailable = await contextforWorkflowType.AnyAsync(a =>
                a.DbTableName.ToUpper() == tableName.ToUpper() && a.IsWorkflowConfigAvailable &&
                a.IsWorkflowDefAvailable);
            return isWorkflowAvailable;
        }

        #endregion

        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var records = await DbSet.Where(predicate).ToListAsync();
            if (!records.Any()) throw new Exception(".NET ObjectNotFoundException"); //new ObjectNotFoundException();
            foreach (var record in records) DbSet.Remove(record);
            return await SaveChangesAsync();
        }

       

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public async Task<long> LongCountAsync()
        {
            return await DbSet.LongCountAsync();
        }

        public async Task<int> CountFuncAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.CountAsync(predicate);
        }

        public async Task<long> LongCountFuncAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.LongCountAsync(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<string> MaxAsync(Expression<Func<TEntity, string>> predicate)
        {
            return await DbSet.MaxAsync(predicate);
        }

        public async Task<string> MaxFuncAsync(Expression<Func<TEntity, string>> predicate,
            Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.Where(where).AsNoTracking().MaxAsync(predicate);
        }

        public async Task<string> MinAsync(Expression<Func<TEntity, string>> predicate)
        {
            return await DbSet.AsNoTracking().MinAsync(predicate);
        }

        public async Task<string> MinFuncAsync(Expression<Func<TEntity, string>> predicate,
            Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.Where(where).AsNoTracking().MinAsync(predicate);
        }

        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var count = await DbSet.CountAsync(predicate);
            return count > 0;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
        }

        public virtual async Task<IList<TResult>> GetAllIncludeAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            var query = DbSet.AsQueryable();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            if (disableTracking)
                query = query.AsNoTracking();

            var result = await query.Select(selector).ToListAsync();

            return result;
        }

        public virtual async Task<(IList<TResult> Items, int Total, int TotalFilter)> GetAllIncludeAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageIndex = 1, int pageSize = 10,
            bool disableTracking = true)
        {
            var query = DbSet.AsQueryable();

            var total = await query.CountAsync();
            var totalFilter = total;

            if (include != null)
                query = include(query);

            if (predicate != null)
            {
                query = query.Where(predicate);
                totalFilter = await query.CountAsync();
            }

            if (orderBy != null)
                query = orderBy(query);

            if (disableTracking)
                query = query.AsNoTracking();

            var result = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(selector).ToListAsync();

            return (result, total, totalFilter);
        }


        public virtual async Task<TResult> GetFirstOrDefaultIncludeAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            var query = DbSet.AsQueryable();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            if (disableTracking)
                query = query.AsNoTracking();

            var result = await query.Select(selector).FirstOrDefaultAsync();

            return result;
        }

        #endregion

        #region SaveChange

        public int SaveChanges()
        {
            int resultLog;
            int result;
            var addedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

            using (var scope = new TransactionScope())
            {
                bool saveFailed;

                do
                {
                    try
                    {
                        resultLog = CreateLog();
                        result = _context.SaveChanges();
                        saveFailed = false;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;
                        resultLog = 0;
                        result = 0;
                        if (ex.Entries != null && ex.Entries.Any())
                            ex.Entries.ToList()
                                .ForEach(entry => { entry.OriginalValues.SetValues(entry.GetDatabaseValues()); });
                    }
                } while (saveFailed);

                scope.Complete();
            }

            return addedEntries.Any() ? resultLog : result;
        }

        public async Task<int> SaveChangesAsync()
        {
            int resultLog;
            int result;
            var addedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                bool saveFailed;
                do
                {
                    try
                    {
                        resultLog = CreateLog();
                        result = await _context.SaveChangesAsync();
                        saveFailed = false;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;
                        result = 0;
                        resultLog = 0;
                        if (ex.Entries != null && ex.Entries.Any())
                            ex.Entries.ToList()
                                .ForEach(entry => { entry.OriginalValues.SetValues(entry.GetDatabaseValues()); });
                    }
                } while (saveFailed);

                scope.Complete();
            }

            return addedEntries.Any() ? resultLog : result;
        }

        private int CreateLog()
        {
            var time = DateTime.Now;
            var userId = 0;
            
            try
            {
                userId = AppIdentity.AppUser.UserId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            var addedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

            if (addedEntries.Count > 0)
                foreach (var entry in addedEntries)
                {
                    if (!(entry.Entity is IAuditableEntity addAudit)) continue;


                    addAudit.CreatedBy = userId;
                    addAudit.CreatedTime = time;
                    addAudit.ModifiedBy = userId;
                    addAudit.ModifiedTime = time;
                }

            var modifiedEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();

            if (modifiedEntries.Count > 0)
                foreach (var entry in modifiedEntries)
                {
                    if (entry.Entity is IAuditableEntity modifyAudit)
                    {
                        modifyAudit.ModifiedBy = userId;
                        modifyAudit.ModifiedTime = time;
                    }

                    var properties = typeof(TEntity).GetProperties()
                        .Where(property =>
                            property != null && Attribute.IsDefined(property, typeof(IgnoreUpdateAttribute)))
                        .Select(p => p.Name);

                    foreach (var property in properties) entry.Property(property).IsModified = false;
                }

            var deleteEntries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
            if (deleteEntries.Count > 0)
                foreach (var entry in deleteEntries)
                {
                    if (entry.Entity is IAuditableEntity deleteAudit)
                    {
                        deleteAudit.ModifiedBy = userId;
                        deleteAudit.ModifiedTime = DateTime.Now;
                    }

                    var properties = typeof(TEntity).GetProperties()
                        .Where(property =>
                            property != null && Attribute.IsDefined(property, typeof(IgnoreUpdateAttribute)))
                        .Select(p => p.Name);
                    foreach (var property in properties) entry.Property(property).IsModified = false;
                }

            if (addedEntries.Count <= 0 && modifiedEntries.Count <= 0 && deleteEntries.Count <= 0) return 0;

            return 1;
        }

        //private static string EntityValidationException(DbEntityValidationException ex)
        //{
        //    var outputLines = new List<string>();
        //    foreach (var eve in ex.EntityValidationErrors)
        //    {
        //        outputLines.Add($"{DateTime.Now:MMM dd, yyyy h:mm tt}: Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:\n");
        //        outputLines.AddRange(eve.ValidationErrors.Select(ve => $"Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\n"));
        //    }


        //    // Retrieve the error messages as a list of strings.
        //    var errorMessages = ex.EntityValidationErrors
        //        .SelectMany(x => x.ValidationErrors)
        //        .Select(x => x.ErrorMessage);

        //    // Join the list to a single string.
        //    var fullErrorMessage = string.Join("; ", errorMessages);

        //    // Combine the original exception message with the new one.
        //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //    //GeneralHelper.WriteValue(string.Join("\n", outputLines));
        //    //GeneralHelper.WriteValue(string.Join("\n", exceptionMessage));


        //    // Throw a new DbEntityValidationException with the improved exception message.
        //    return exceptionMessage;
        //}

        #endregion

        #region DailyCMActivity

        public async Task<(DailyCMActivity Data, bool IsExists)> UpdateDailyCMActivityAsync(DailyCMActivity entity,
            DailyCMActivity existingEntity)
        {
            // var dbEntity = await _context.FindAsync<TEntity>(entity.Id);
            var savedEntity = new DailyCMActivity();
            if (existingEntity != null)
            {
                savedEntity = await ModifyCMTask(entity, existingEntity);
            }
            else
            {
                // check duplicate
                var dupEntity = await _context.Set<DailyCMActivity>().AsNoTracking().AnyAsync(x =>
                    x.OutletId == entity.OutletId &&
                    x.CMId == entity.CMId && x.Date.Date == entity.Date.Date);
                if (dupEntity) return (null, true);

                savedEntity = await InsertCMTask(entity);
            }

            return (savedEntity, false);
        }

        private async Task<DailyCMActivity> ModifyCMTask(DailyCMActivity entity, DailyCMActivity existingEntity)
        {
            if (!entity.IsPOSM) entity.DailyPOSM = null;
            if (!entity.IsAudit) entity.DailyAudit = null;

            var dbEntry = _context.Entry(existingEntity);
            dbEntry.CurrentValues.SetValues(entity);
            dbEntry.State = EntityState.Modified;

            #region SurveyQuestions

            var dbItemsMap = existingEntity.SurveyQuestions.ToDictionary(e => e.Id);

            foreach (var item in entity.SurveyQuestions)
                if (!dbItemsMap.TryGetValue(item.Id, out var oldItem))
                {
                    _context.Entry(item).State = EntityState.Added;
                }
                else
                {
                    _context.Entry(oldItem).CurrentValues.SetValues(item);
                    _context.Entry(oldItem).State = EntityState.Modified;
                    dbItemsMap.Remove(item.Id);
                }

            foreach (var oldItem in dbItemsMap.Values)
                _context.Entry(oldItem).State = EntityState.Deleted;

            #endregion

            #region DailyPOSM

            if (existingEntity.DailyPOSM != null)
            {
                _context.Entry(existingEntity.DailyPOSM).CurrentValues.SetValues(entity.DailyPOSM);
                _context.Entry(existingEntity.DailyPOSM).State = EntityState.Modified;

                var dbItemsMapChild = existingEntity.DailyPOSM.POSMProducts.ToDictionary(e => e.Id);

                foreach (var item in entity.DailyPOSM.POSMProducts)
                    if (!dbItemsMapChild.TryGetValue(item.Id, out var oldItem))
                    {
                        _context.Entry(item).State = EntityState.Added;
                    }
                    else
                    {
                        _context.Entry(oldItem).CurrentValues.SetValues(item);
                        _context.Entry(oldItem).State = EntityState.Modified;
                        dbItemsMapChild.Remove(item.Id);
                    }

                foreach (var oldItem in dbItemsMapChild.Values)
                    _context.Entry(oldItem).State = EntityState.Deleted;
            }
            else if (entity.DailyPOSM != null)
            {
                var dbEntryDailyPOSM = _context.Entry(entity.DailyPOSM);
                dbEntryDailyPOSM.State = EntityState.Added;
            }

            #endregion

            #region DailyAudit

            if (existingEntity.DailyAudit != null)
            {
                _context.Entry(existingEntity.DailyAudit).CurrentValues.SetValues(entity.DailyAudit);
                _context.Entry(existingEntity.DailyAudit).State = EntityState.Modified;

                var dbItemsMapChild = existingEntity.DailyAudit.AllProducts.ToDictionary(e => e.Id);

                foreach (var item in entity.DailyAudit.AllProducts)
                    if (!dbItemsMapChild.TryGetValue(item.Id, out var oldItem))
                    {
                        _context.Entry(item).State = EntityState.Added;
                    }
                    else
                    {
                        _context.Entry(oldItem).CurrentValues.SetValues(item);
                        _context.Entry(oldItem).State = EntityState.Modified;
                        dbItemsMapChild.Remove(item.Id);
                    }

                foreach (var oldItem in dbItemsMapChild.Values)
                    _context.Entry(oldItem).State = EntityState.Deleted;
            }
            else if (entity.DailyAudit != null)
            {
                var dbEntryChild = _context.Entry(entity.DailyAudit);
                dbEntryChild.State = EntityState.Added;
            }

            #endregion

            await _context.SaveChangesAsync();
            return entity;
        }

        private async Task<DailyCMActivity> InsertCMTask(DailyCMActivity entity)
        {
            if (entity.DailyPOSM != null)
            {
                entity.DailyPOSM.POSMInstallationStatus = (int) Status.Pending;
                entity.DailyPOSM.POSMRepairStatus = (int) Status.Pending;
                entity.DailyPOSM.POSMRemovalStatus = (int) Status.Pending;
            }

            if (entity.DailyAudit != null)
            {
                entity.DailyAudit.DistributionCheckStatus = (int) Status.Pending;
                entity.DailyAudit.FacingCountStatus = (int) Status.Pending;
                entity.DailyAudit.PlanogramCheckStatus = (int) Status.Pending;
                entity.DailyAudit.PriceAuditCheckStatus = (int) Status.Pending;
            }

            if (entity.SurveyQuestions != null) entity.SurveyQuestions.ForEach(x => x.Status = Status.Pending);
            entity.Status = Status.Pending;
            //_context.Add(entity);
            //await _context.SaveChangesAsync();

            #region workflow log

            var tableName = _context.Model.FindEntityType(typeof(DailyCMActivity)).GetTableName();
            _context.Add(entity);
            await _context.SaveChangesAsync();

            #endregion

            // DailyPOSM
            if (entity.DailyPOSM != null)
                foreach (var item in entity.DailyPOSM.POSMProducts)
                {
                    item.DailyCMActivityId = entity.Id;
                    item.Status = Status.Pending;
                    _context.Entry(item).State = EntityState.Modified;
                }

            // DailyAudit
            if (entity.DailyAudit != null)
                foreach (var item in entity.DailyAudit.AllProducts)
                {
                    item.DailyCMActivityId = entity.Id;
                    item.Status = Status.Pending;
                    _context.Entry(item).State = EntityState.Modified;
                }

            await _context.SaveChangesAsync();
            return entity;
        }

        #endregion

        #region Node wise user

        public IList<UserInfo> GetNodeWiseUsersByUserId(int userId, bool isOnlyLastNodeUser = false)
        {
            var userTerrMaps = _context.Set<UserTerritoryMapping>();
            var nodes = _context.Set<Node>();
            var users = _context.Set<UserInfo>();

            #region get all child node

            var nodeIds = userTerrMaps.Where(s => s.UserInfoId == userId).Select(x => x.NodeId).ToList();
            var allNodes = nodes.ToList();
            var territoryNodeIds = new List<int>();

            // get all territory node ids
            foreach (var nodeId in nodeIds)
                territoryNodeIds.AddRange(GetAllChildNodeIds(allNodes, nodeId, new List<int>(), isOnlyLastNodeUser)
                    .ChildNodeIds);
            territoryNodeIds = territoryNodeIds.Distinct().ToList();

            #endregion

            var userList = users.Where(x => x.Id == userId ||
                                            x.Id != userId && x.Territories.Any(c =>
                                                territoryNodeIds.Any(tId => tId == c.NodeId))).ToList();

            return userList;
        }

        private (IList<Node> AllNodes, int NodeId, IList<int> ChildNodeIds, bool isOnlyLastNodeUser) GetAllChildNodeIds(
            IList<Node> AllNodes, int NodeId, IList<int> ChildNodeIds, bool isOnlyLastNodeUser)
        {
            if (!isOnlyLastNodeUser) ChildNodeIds.Add(NodeId);
            if (!AllNodes.Any(x => x.ParentId == NodeId))
            {
                if (isOnlyLastNodeUser) ChildNodeIds.Add(NodeId);
                return (AllNodes, NodeId, ChildNodeIds, isOnlyLastNodeUser);
            }

            var nodes = AllNodes.Where(x => x.ParentId == NodeId).ToList();
            foreach (var node in nodes) GetAllChildNodeIds(AllNodes, node.NodeId, ChildNodeIds, isOnlyLastNodeUser);

            return (AllNodes, NodeId, ChildNodeIds, isOnlyLastNodeUser);
        }

        #endregion


        #region Custom Pagination

        public async Task<List<TEntity>> ListAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetEntityWithSpec(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> ListAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_context.Set<TEntity>().AsQueryable(), spec);
        }

        #endregion
    }
}