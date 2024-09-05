using System;
using System.Collections.Generic;
using System.Text;

namespace attendance_management_app.Services
{
    public class GelocationService
    {

        public static readonly double _empresaLatitud = -12.0003777;
        public static readonly double _empresaLongitud = -76.8611125;
        public static readonly double _rangoPermitido = 0.01;

        // Método para calcular la distancia en kilómetros
        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double dLat = (lat2 - lat1) * Math.PI / 180.0;
            double dLon = (lon2 - lon1) * Math.PI / 180.0;

            // Convertir grados a radianes
            lat1 = lat1 * Math.PI / 180.0;
            lat2 = lat2 * Math.PI / 180.0;

            // Aplicar fórmula del haversine
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Radio de la Tierra en km
            double r = 6371;

            // Retornar distancia en kilómetros
            return r * c;
        }

        public static bool OnLocationReceived(double latitude, double longitude)
        {
            var distancia = CalculateDistance(latitude, longitude, _empresaLatitud, _empresaLongitud);
            return distancia <= _rangoPermitido;
        }

    }
}
