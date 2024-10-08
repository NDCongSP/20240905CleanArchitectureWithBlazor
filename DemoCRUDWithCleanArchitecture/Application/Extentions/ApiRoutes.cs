﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extentions
{
    public static class ApiRoutes
    {
        /// <summary>
        /// httpGet.
        /// </summary>
        public const string GetAll = "";
        /// <summary>
        /// httpget.
        /// </summary>
        public const string GetById = "{id}";
        /// <summary>
        /// httppost.
        /// </summary>
        public const string Update = "update";
        /// <summary>
        /// httppost.
        /// </summary>
        public const string Insert = "insert";
        public const string Delete = "DELETE";

        public static class Identity
        {
            public const string BasePath = "api/account";
            public const string Login = "identity/login";
            public const string CreateAccount = "identity/create";
            public const string RefreshToken = "identity/refresh-token";
            public const string RoleCreate = "identity/role/create";
            public const string RoleList = "identity/role/list";
            public const string CreateAdminAccount = "identity/setting";
            public const string UserWithRole = "identity/user-with-role";
            public const string ChangePassword = "identity/change-pass";
            public const string ChangrRole = "identity/change-role";
            public const string AssignUserRole = "identity/assign_user_role";
            //public const string GetFromToByName = "GetByName/{from}/{to}/{tenChuong}";
        }

        public static class Product
        {
            public const string BasePath = "api/Products";
            public const string GetFillter = "GetFillter";
        }

        public static class Unit
        {
            public const string BasePath = "api/units";
        }
    }
}
