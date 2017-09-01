using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Interface;

namespace Backend.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IUserService _userService;

        public ImageController(IUserService userService)
        {
            _userService = userService;
        }

        public FileContentResult UserImage(int id)
        {
            var image = _userService.GetUserImage(id <= 0 ? _userService.UserId : id);

            return File(image, "image/jpeg");
        }
    }
}