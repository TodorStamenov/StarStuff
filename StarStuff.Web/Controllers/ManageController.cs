namespace StarStuff.Web.Controllers
{
    using Data;
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.Manage;
    using Services;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger logger;
        private readonly IUserService userService;

        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public ManageController(
          UserManager<User> userManager,
          SignInManager<User> signInManager,
          ILogger<ManageController> logger,
          IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.userService = userService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> ApplyForAstronomer()
        {
            User user = await userManager.GetUserAsync(User);

            bool isAstronomer = await userManager.IsInRoleAsync(user, WebConstants.AstronomerRole);

            if (isAstronomer)
            {
                return BadRequest();
            }

            ApplyForAstronomerViewModel model = new ApplyForAstronomerViewModel
            {
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> ApplyForAstronomer(ApplyForAstronomerViewModel model)
        {
            User user = await userManager.GetUserAsync(User);

            bool isAstronomer = await userManager.IsInRoleAsync(user, WebConstants.AstronomerRole);

            if (isAstronomer)
            {
                StatusMessage = "You are already astronomer";
                return RedirectToAction(nameof(Index));
            }

            bool hasFullData = user.FirstName != null
                && user.LastName != null
                && user.Email != null
                && user.BirthDate != null
                && user.PhoneNumber != null;

            if (!hasFullData)
            {
                StatusMessage = "You have to give all personal data";
                return RedirectToAction(nameof(Index));
            }

            if (user.SendApplication)
            {
                StatusMessage = "You have to wait for approval";
                return RedirectToAction(nameof(Index));
            }

            bool success = this.userService.Apply(user.Id);

            if (!success)
            {
                return BadRequest();
            }

            StatusMessage = "Your application has been sent successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await userManager.GetUserAsync(User);

            IndexViewModel model = new IndexViewModel
            {
                Username = user.UserName,
                ProfileImage = this.ConvertUserImage(user.ProfileImage),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            User user = await userManager.GetUserAsync(User);

            model.ProfileImage = this.ConvertUserImage(user.ProfileImage);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = user.Id;

            bool success = true;

            if (model.Image != null)
            {
                if (!model.Image.ContentType.Contains("image")
                        || model.Image.Length > DataConstants.UserConstants.MaxImageSize)
                {
                    TempData.AddErrorMessage("Uploaded image file must be less then 1 MB");
                    return RedirectToAction(nameof(Index));
                }

                success = this.userService
                    .AddProfileImage(userId, model.Image.ToByteArray());

                if (!success)
                {
                    return BadRequest();
                }
            }

            success = this.userService.UpdateUser(
                userId,
                model.FirstName,
                model.LastName,
                model.Email,
                model.BirthDate,
                model.PhoneNumber);

            if (!success)
            {
                return BadRequest();
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var hasPassword = await userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction(nameof(SetPassword));
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var hasPassword = await userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToAction(nameof(ChangePassword));
            }

            var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                AddErrors(addPasswordResult);
                return View(model);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your password has been set.";

            return RedirectToAction(nameof(SetPassword));
        }

        #region Helpers

        private string ConvertUserImage(byte[] profileImage)
        {
            return WebConstants.DataImage + Convert.ToBase64String(profileImage);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        #endregion Helpers
    }
}