namespace CameraBazaar.Web.Controllers
{
    using System.Linq;
    using CameraBazaar.Data.Models;
    using CameraBazaar.Services;
    using CameraBazaar.Web.Infrastructure.Filters;
    using CameraBazaar.Web.Models.Cameras;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CamerasController : Controller
    {
        private readonly ICameraService cameras;
        private readonly UserManager<User> userManager;

        public CamerasController(
            UserManager<User> userManager,
            ICameraService cameras)
        {
            this.cameras = cameras;
            this.userManager = userManager;
        }

        [Authorize]
        [MeasureTime]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Add(CameraFormModel cameraModel)
        {
            if (cameraModel.LightMeterings==null || !cameraModel.LightMeterings.Any())
            {
                ModelState.AddModelError(nameof(cameraModel.LightMeterings), "Please select at least one light metering.");
            }

            if (!ModelState.IsValid)
            {
                return this.View(cameraModel);
            }

            this.cameras.Create(
                cameraModel.Make,
                cameraModel.Model,
                cameraModel.Price,
                cameraModel.Quantity,
                cameraModel.MinShutterSpeed,
                cameraModel.MaxShutterSpeed,
                cameraModel.MinISO,
                cameraModel.MaxISO,
                cameraModel.IsFullFrame,
                cameraModel.VideoResolution,
                cameraModel.LightMeterings,
                cameraModel.Description,
                cameraModel.ImageUrl,
                this.userManager.GetUserId(User));

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [Authorize]
        public IActionResult Edit(int id)
        {
            // is this camera created by this.User ?
            var cameraExists = this.cameras.Exists(id, this.userManager.GetUserId(this.User));

            if (!cameraExists)
            {
                return this.NotFound();
            }

            // TODO:
            return this.View();
        }


        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, CameraFormModel cameraModel)
        {
            //// id of the user
            //string userId = "sdfsdfsdfsd";
            //// is user, who try to edit is same as the currently loged user ?
            //var allowed = (userId == this.userManager.GetUserId(this.User));
            //if (!allowed)
            //{
            //    return this.Unauthorized();
            //}

            // is this camera created by this.User ?
            var cameraExists = this.cameras.Exists(id, this.userManager.GetUserId(this.User));

            if (!cameraExists)
            {
                return this.NotFound();
            }

            if (cameraModel.LightMeterings == null || !cameraModel.LightMeterings.Any())
            {
                ModelState.AddModelError(nameof(cameraModel.LightMeterings), "Please select at least one light metering.");
            }

            if (!ModelState.IsValid)
            {
                return this.View(cameraModel);
            }

            var updated = this.cameras.Edit(
                id,
                cameraModel.Make,
                cameraModel.Model,
                cameraModel.Price,
                cameraModel.Quantity,
                cameraModel.MinShutterSpeed,
                cameraModel.MaxShutterSpeed,
                cameraModel.MinISO,
                cameraModel.MaxISO,
                cameraModel.IsFullFrame,
                cameraModel.VideoResolution,
                cameraModel.LightMeterings,
                cameraModel.Description,
                cameraModel.ImageUrl);

            if (!updated)
            {
                return this.NotFound();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}