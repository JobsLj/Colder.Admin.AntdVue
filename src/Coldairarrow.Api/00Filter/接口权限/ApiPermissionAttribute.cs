﻿using Coldairarrow.Business;
using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Coldairarrow.Api
{
    /// <summary>
    /// 接口权限校验
    /// </summary>
    public class ApiPermissionAttribute : BaseActionFilterAsync
    {
        readonly IPermissionBusiness _permissionBus;
        readonly IOperator _operator;
        public ApiPermissionAttribute(IPermissionBusiness permissionBus, IOperator @operator, string permissionValue)
        {
            _permissionBus = permissionBus;
            _operator = @operator;

            if (permissionValue.IsNullOrEmpty())
                throw new Exception("permissionValue不能为空");

            _permissionValue = permissionValue;
        }

        public string _permissionValue { get; }

        /// <summary>
        /// Action执行之前执行
        /// </summary>
        /// <param name="context">过滤器上下文</param>
        public async override Task OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ContainsFilter<NoApiPermissionAttribute>())
                return;

            var permissions = await _permissionBus.GetUserPermissionValuesAsync(_operator.UserId);
            if (!permissions.Contains(_permissionValue))
                context.Result = Error("权限不足!");
        }
    }
}