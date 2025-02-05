﻿// <copyright file="Eg01CreateClickwrapController.cs" company="DocuSign">
// Copyright (c) DocuSign. All rights reserved.
// </copyright>

namespace DocuSign.CodeExamples.Click.Controllers
{
    using DocuSign.Click.Client;
    using DocuSign.Click.Examples;
    using DocuSign.CodeExamples.Common;
    using DocuSign.CodeExamples.Controllers;
    using DocuSign.CodeExamples.Models;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Area("Click")]
    [Route("ClickEg01")]
    public class CreateClickwrap : EgController
    {
        public CreateClickwrap(DSConfiguration config, LauncherTexts launcherTexts, IRequestItemsService requestItemsService)
            : base(config, launcherTexts, requestItemsService)
        {
            this.CodeExampleText = this.GetExampleText(EgName);
            this.ViewBag.title = this.CodeExampleText.ExampleName;
        }

        public override string EgName => "ClickEg01";

        [MustAuthenticate]
        [SetViewBag]
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name)
        {
            // Obtain your OAuth token
            var accessToken = this.RequestItemsService.User.AccessToken;
            var basePath = $"{this.RequestItemsService.Session.BasePath}/clickapi"; // Base API path
            var accountId = this.RequestItemsService.Session.AccountId;

            try
            {
                // Call the Click API to create a clickwrap
                var clickWrap = DocuSign.Click.Examples.CreateClickwrap.Create(name, basePath, accessToken, accountId);

                // Show results
                this.ViewBag.h1 = this.CodeExampleText.ExampleName;
                this.ViewBag.message = this.CodeExampleText.ResultsPageText.Replace("{0}", clickWrap.ClickwrapName);
                this.ViewBag.Locals.Json = JsonConvert.SerializeObject(clickWrap, Formatting.Indented);

                // Save for future use within the example launcher
                this.RequestItemsService.ClickwrapId = clickWrap.ClickwrapId;
                this.RequestItemsService.ClickwrapName = clickWrap.ClickwrapName;

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