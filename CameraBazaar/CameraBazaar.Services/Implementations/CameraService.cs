namespace CameraBazaar.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using CameraBazaar.Data;
    using CameraBazaar.Data.Models;
    using Remotion.Linq.Parsing.Structure.IntermediateModel;

    public class CameraService : ICameraService
    {
        private readonly CameraBazaarDbContext db;

        public CameraService(CameraBazaarDbContext db)
        {
            this.db = db;
        }

        public void Create(
            CameraMakeType make,
            string model,
            decimal price,
            int quantity,
            int minShutterSpeed,
            int maxShutterSpeed,
            MinISO minISO,
            int maxISO,
            bool isFullFrame,
            string videoResolution,
            IEnumerable<LightMetering> lightMeterings,
            string description,
            string imageUrl,
            string userId)
        {
            // ANOTHER WAY
            //LightMetering lightMetering = lightMeterings.First();
            //foreach (LightMetering lightMeteringValue in lightMeterings.Skip(1))
            //{
            //    lightMetering |= lightMeteringValue;
            //}

            Camera camera = new Camera
            {
                Make = make,
                Model = model,
                Price = price,
                Quantity = quantity,
                MinShutterSpeed = minShutterSpeed,
                MaxShutterSpeed = maxShutterSpeed,
                MinISO = minISO,
                MaxISO = maxISO,
                IsFullFrame = isFullFrame,
                VideoResolution = videoResolution,
                LightMetering = (LightMetering)lightMeterings.Cast<int>().DefaultIfEmpty().Sum(),
                Description = description,
                ImageUrl = imageUrl,
                UserId = userId
            };

            this.db.Cameras.Add(camera);
            this.db.SaveChanges();
        }


        public bool Edit(
            int id,
            CameraMakeType make,
            string model,
            decimal price,
            int quantity,
            int minShutterSpeed,
            int maxShutterSpeed,
            MinISO minISO,
            int maxISO,
            bool isFullFrame,
            string videoResolution,
            IEnumerable<LightMetering> lightMeterings,
            string description,
            string imageUrl)
        {
            var camera = this.db.Cameras.Find(id);

            if (camera==null)
            {
                return false;
            }

            camera.Make = make;
            camera.Model = model;
            camera.Price = price;
            camera.Quantity = quantity;
            camera.MinShutterSpeed = minShutterSpeed;
            camera.MaxShutterSpeed = maxShutterSpeed;
            camera.MinISO = minISO;
            camera.MaxISO = maxISO;
            camera.IsFullFrame = isFullFrame;
            camera.VideoResolution = videoResolution;
            camera.LightMetering = (LightMetering) lightMeterings.Cast<int>().DefaultIfEmpty().Sum();
            camera.Description = description;
            camera.ImageUrl = imageUrl;

            this.db.SaveChanges();

            return true;
        }


        public bool Exists(int id, string userId)
        {
            return this.db.Cameras.Any(c => c.Id == id && c.UserId == userId);
        }
    }
}