﻿namespace StarStuff.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Publications;
    using StarStuff.Data.Models;
    using StarStuff.Services;
    using StarStuff.Services.Moderator;
    using System;

    public class PublicationsController : Controller
    {
        private const int PublicationsPerPage = 10;
        private const int CommentsPerPage = 10;

        private readonly ICommentService commentService;
        private readonly IJournalService journalService;
        private readonly ITelescopeService telescopeService;
        private readonly IPublicationService publicationService;
        private readonly UserManager<User> userManager;

        public PublicationsController(
            ICommentService commentService,
            IJournalService journalService,
            ITelescopeService telescopeService,
            IPublicationService publicationService,
            UserManager<User> userManager)
        {
            this.commentService = commentService;
            this.journalService = journalService;
            this.telescopeService = telescopeService;
            this.publicationService = publicationService;
            this.userManager = userManager;
        }

        public IActionResult Details(int id, int page)
        {
            if (page <= 0)
            {
                page = 1;
            }

            int? userId = null;

            if (User.Identity.IsAuthenticated)
            {
                userId = int.Parse(this.userManager.GetUserId(User));
            }

            PublicationDetailsViewModel model = new PublicationDetailsViewModel
            {
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(this.commentService.Total(id) / (double)CommentsPerPage),
                Publication = this.publicationService.Details(id),
                Comments = this.commentService.All(id, page, CommentsPerPage, userId)
            };

            if (model.Publication == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        public IActionResult All(int page)
        {
            if (page <= 0)
            {
                page = 1;
            }

            ListPublicationsViewModel model = new ListPublicationsViewModel
            {
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(this.publicationService.Total() / (double)PublicationsPerPage),
                Publications = this.publicationService.All(page, PublicationsPerPage)
            };

            return View(model);
        }

        public IActionResult ByJournal(int page, int journalId)
        {
            if (page <= 0)
            {
                page = 1;
            }

            ListPublicationsByJournalViewModel model = new ListPublicationsByJournalViewModel
            {
                JournalId = journalId,
                JournalName = this.journalService.GetName(journalId),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(this.publicationService.TotalByJournal(journalId) / (double)PublicationsPerPage),
                Publications = this.publicationService.AllByJournal(journalId, page, PublicationsPerPage)
            };

            if (model.Publications == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        public IActionResult ByTelescope(int page, int telescopeId)
        {
            if (page <= 0)
            {
                page = 1;
            }

            ListPublicationsByTelescopeViewModel model = new ListPublicationsByTelescopeViewModel
            {
                TelescopeId = telescopeId,
                TelescopeName = this.telescopeService.GetName(telescopeId),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(this.publicationService.TotalByTelescope(telescopeId) / (double)PublicationsPerPage),
                Publications = this.publicationService.AllByTelescope(telescopeId, page, PublicationsPerPage)
            };

            if (model.Publications == null)
            {
                return BadRequest();
            }

            return View(model);
        }
    }
}