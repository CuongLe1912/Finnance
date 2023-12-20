using System;
using System.Collections.Generic;
using System.Text;

namespace URF.Core.Helper.Helpers
{
    public class Coordinates
    {
        public double? Latitude { get; private set; }
        public double? Longitude { get; private set; }

        public Coordinates(double? latitude, double? longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
    public class UnitOfLength
    {
        private readonly double _fromMilesFactor;
        public static UnitOfLength Miles = new UnitOfLength(1);
        public static UnitOfLength Kilometers = new UnitOfLength(1.609344);
        public static UnitOfLength NauticalMiles = new UnitOfLength(0.8684);

        private UnitOfLength(double fromMilesFactor)
        {
            _fromMilesFactor = fromMilesFactor;
        }

        public double ConvertFromMiles(double input)
        {
            return input * _fromMilesFactor;
        }
    }
    public static class CoordinatesDistanceExtensions
    {
        public static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates)
        {
            return DistanceTo(baseCoordinates, targetCoordinates, UnitOfLength.Kilometers);
        }

        public static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates, UnitOfLength unitOfLength)
        {
            if (!baseCoordinates.Latitude.HasValue || !baseCoordinates.Longitude.HasValue)
                return 0D;

            var baseRad = Math.PI * baseCoordinates.Latitude.Value / 180;
            var targetRad = Math.PI * targetCoordinates.Latitude.Value / 180;
            var theta = baseCoordinates.Longitude.Value - targetCoordinates.Longitude.Value;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return unitOfLength.ConvertFromMiles(dist);
        }
    }
}
