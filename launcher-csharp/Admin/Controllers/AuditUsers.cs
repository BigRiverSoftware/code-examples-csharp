﻿// <copyright file="Eg05AuditUsersController.cs" company="DocuSign">
// Copyright (c) DocuSign. All rights reserved.
// </copyright>

namespace DocuSign.CodeExamples.Admin.Controllers
{
    using System;
    using DocuSign.Admin.Client;
    using DocuSign.Admin.Examples;
    using DocuSign.CodeExamples.Common;
    using DocuSign.CodeExamples.Controllers;
    using DocuSign.CodeExamples.Models;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Area("Admin")]
    [Route("Aeg05")]
    public class AuditUsers : EgController
    {
        public AuditUsers(
            DSConfiguration dsConfig,
            LauncherTexts launcherTexts,
            IRequestItemsService requestItemsService)
            : base(dsConfig, launcherTexts, requestItemsService)
        {
            this.CodeExampleText = this.GetExampleText(EgName);
            this.ViewBag.title = this.CodeExampleText.ExampleName;
        }

        public override string EgName => "Aeg05";

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            this.ViewBag.AccountId = this.RequestItemsService.Session.AccountId;
        }

        [MustAuthenticate]
        [SetViewBag]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Audit()
        {
            try
            {
                var organizationId = RequestItemsService.OrganizationId;
                var accessToken = RequestItemsService.User.AccessToken;
                var basePath = RequestItemsService.Session.AdminApiBasePath;
                var accountId = RequestItemsService.Session.AccountId;
                var usersData = DocuSign.Admin.Examples.AuditUsers.GetRecentlyModifiedUsersData(basePath, accessToken, Guid.Parse(accountId), organizationId);
                // Process results
                this.ViewBag.h1 = this.CodeExampleText.ExampleName;
                this.ViewBag.message = this.CodeExampleText.ResultsPageText;
                this.ViewBag.Locals.Json = JsonConvert.SerializeObject(usersData, Formatting.Indented);
                return this.View("example_done");
            }
            catch (ApiException apiException)
            {
                this.ViewBag.errorCode = apiException.ErrorCode;
                this.ViewBag.errorMessage = apiException.Message;

                return this.View("Error");
            }
        }
    }
}