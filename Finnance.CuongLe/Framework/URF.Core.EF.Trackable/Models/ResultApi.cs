using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable.Enums;

namespace URF.Core.EF.Trackable.Models
{
    public class ResultApi
    {
        public bool Success
        {
            get
            {
                return Type == ResultType.Success;
            }
        }
        public object Object { get; set; }
        public decimal? Total { get; set; }
        public ResultType Type { get; set; }
        public object ObjectExtra { get; set; }
        public string Description { get; set; }
        public object ObjectStatistical { get; set; }

        public ResultApi(object obj = null, ResultType type = ResultType.Success, string description = default)
        {
            Type = type;
            Object = obj;
            Description = description;
        }

        public static bool IsSuccess(ResultApi result)
        {
            return result != null && result.Type == ResultType.Success;
        }
        public static ResultApi ToException(Exception ex)
        {
            return new ResultApi
            {
                Object = ex,
                Type = ResultType.Exception
            };
        }
        public static ResultApi ToExceptionApi(Exception ex)
        {
            return new ResultApi
            {
                Object = null,
                ObjectExtra = ex,
                Type = ResultType.Fail,
                Description = "Lỗi kết nối hệ thống API",
            };
        }
        public static ResultApi ToException(string description)
        {
            return new ResultApi
            {
                Description = description,
                Type = ResultType.Exception
            };
        }
        public static ResultApi ToError(string description = default, object extra = null)
        {
            return new ResultApi
            {
                Object = null,
                ObjectExtra = extra,
                Type = ResultType.Fail,
                Description = description,
            };
        }
        public static ResultApi ToSuccess(string description = default, object extra = null)
        {
            return new ResultApi
            {
                Object = null,
                ObjectExtra = extra,
                Type = ResultType.Success,
                Description = description,
            };
        }
        public static ResultApi ToEntity(object item = null, object extra = null, decimal? total = null)
        {
            if (item != null)
            {
                return new ResultApi
                {
                    Total = total,
                    Object = item,
                    ObjectExtra = extra,
                    Type = ResultType.Success,
                    Description = string.Empty,
                };
            }
            else
            {
                return new ResultApi
                {
                    Object = null,
                    Total = total,
                    ObjectExtra = extra,
                    Type = ResultType.Success,
                    Description = string.Empty,
                };
            }
        }
        public static ResultApi ToEntity<TEntity>(List<TEntity> items, object extra = null, decimal? total = null) where TEntity : Entity
        {
            if (items != null)
            {
                return new ResultApi
                {
                    Object = items,
                    ObjectExtra = extra,
                    Type = ResultType.Success,
                    Description = string.Empty,
                    Total = total,
                };
            }
            else
            {
                return new ResultApi
                {
                    Object = null,
                    Type = ResultType.Success,
                    Description = string.Empty,
                    Total = total,
                };
            }
        }
        public static ResultApi ToEntityStatistical(object item = null, object extra = null, object statistical = null)
        {
            if (item != null)
            {
                return new ResultApi
                {
                    Object = item,
                    ObjectExtra = extra,
                    Type = ResultType.Success,
                    Description = string.Empty,
                    ObjectStatistical = statistical,
                };
            }
            else
            {
                return new ResultApi
                {
                    Object = null,
                    ObjectExtra = extra,
                    Type = ResultType.Success,
                    Description = string.Empty,
                    ObjectStatistical = statistical,
                };
            }
        }
        public static ResultApi ToMongoEntity<TEntity>(List<TEntity> items, object extra = null, decimal? total = null) where TEntity : MongoEntity
        {
            if (items != null)
            {
                return new ResultApi
                {
                    Object = items,
                    ObjectExtra = extra,
                    Type = ResultType.Success,
                    Description = string.Empty,
                    Total = total,
                };
            }
            else
            {
                return new ResultApi
                {
                    Object = null,
                    Type = ResultType.Success,
                    Description = string.Empty,
                    Total = total,
                };
            }
        }
    }
    public class ResultLiteApi
    {
        public object Object { get; set; }
        public ResultType Type { get; set; }
        public string Description { get; set; }

        public ResultLiteApi(ResultApi result)
        {
            if (result != null)
            {
                Type = result.Type;
                Object = result.Object;
                Description = result.Description;
            }
        }
        public ResultLiteApi(object obj = null, ResultType type = ResultType.Success, string description = default)
        {
            Type = type;
            Object = obj;
            Description = description;
        }

        public static ResultLiteApi ToException(Exception ex)
        {
            return new ResultLiteApi
            {
                Object = ex,
                Type = ResultType.Exception
            };
        }
        public static ResultLiteApi ToException(string description)
        {
            return new ResultLiteApi
            {
                Description = description,
                Type = ResultType.Exception
            };
        }
        public static ResultLiteApi ToError(string description = default)
        {
            return new ResultLiteApi
            {
                Object = null,
                Type = ResultType.Fail,
                Description = description,
            };
        }
        public static ResultLiteApi ToSuccess(string description = default)
        {
            return new ResultLiteApi
            {
                Object = null,
                Type = ResultType.Success,
                Description = description,
            };
        }

        public static ResultLiteApi ToEntity(object item = null)
        {
            if (item != null)
            {
                return new ResultLiteApi
                {
                    Object = item,
                    Type = ResultType.Success,
                    Description = string.Empty,
                };
            }
            else
            {
                return new ResultLiteApi
                {
                    Object = null,
                    Type = ResultType.Success,
                    Description = string.Empty,
                };
            }
        }
        public static ResultLiteApi ToEntity<TEntity>(List<TEntity> items) where TEntity : Entity
        {
            if (items != null)
            {
                return new ResultLiteApi
                {
                    Object = items,
                    Type = ResultType.Success,
                    Description = string.Empty,
                };
            }
            else
            {
                return new ResultLiteApi
                {
                    Object = null,
                    Type = ResultType.Success,
                    Description = string.Empty,
                };
            }
        }
    }
}
